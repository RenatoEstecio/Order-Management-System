using EFCore;
using Library.DTO;
using Library.Enums;
using Library.Repository;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.ResponseDTO.Pedido;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Library.BLL
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly IClienteRepository _repositoryCliente;
        private readonly IPedidoItemRepository _repositoryPedidoItem;
        private readonly Repository.IPedidoHistoricoRepository _pedidoHistoricoRepository;

        public PedidoService(
            IPedidoRepository repository, 
            IClienteRepository repositoryCliente, 
            IPedidoItemRepository repositoryPedidoItem,
            Repository.IPedidoHistoricoRepository pedidoHistoricoRepository)
        {
            _repository = repository;
            _repositoryCliente = repositoryCliente;
            _repositoryPedidoItem = repositoryPedidoItem;
            _pedidoHistoricoRepository = pedidoHistoricoRepository;
        }
        public async Task<PedidoGetDetailsResponse> Buscar(Guid id)
        {
            PedidoGetDetailsResponse response = new PedidoGetDetailsResponse();

            Pedido? pedido = await _repository.ObterPedido(id);
            Cliente? cliente = await _repositoryCliente.Buscar(pedido.ClienteId);
            response.Produtos = await _repositoryPedidoItem.ObterProdutos(pedido.PedidoId);

            response.Cliente = new ClienteListResponse(cliente);
            response.Total = response.Produtos.Sum(x => x.Total);
            response.CreatedAt = pedido.CreatedAt;
            response.UpdatedAt = pedido.UpdatedAt;
            response.Status = ((StatusPedido)pedido.PedidoStatusId).ToString();
            response.Id = pedido.Id;

            List<PedidoHistorico> listHistorico = await _pedidoHistoricoRepository.ObterHistorico(pedido.PedidoId);

            string? statusAnterior = null;

            foreach (var item in listHistorico)
            {
                HistoricoListResponse historico = new HistoricoListResponse();
                historico.Horario = item.CreatedAt;
                string status = ((StatusPedido)item.PedidoStatusId).ToString();

                if (statusAnterior == null)
                    historico.Mensagem = status;
                else
                    historico.Mensagem = $"Status alterado de {statusAnterior} para {status}";

                if(item.Motivo is not null)
                    historico.Mensagem += $". Motivo: {item.Motivo.ToString()}";

                statusAnterior = status;

                response.Historico.Add(historico);
            }

            response.Message = "Sucesso";

            return response;
        }
        public async Task ExecutarPedido(          
            ListaPedidoRequest pedidoLista)
        {

            Cliente? cliente = await _repositoryCliente.Buscar(pedidoLista.cliente);

            if(cliente == null)
                throw new CustomException("Cliente não encontrado", HttpStatusCode.BadRequest);

            if (cliente == null || cliente.Ativo == false)
                throw new CustomException("Cliente não autorizado", HttpStatusCode.Unauthorized);

            await using var transaction =
                await _repository.BeginTransaction();           

            try
            {
                Pedido pedido = new();

                /*Agrupando quantidade caso exista ids repetidos */
                pedidoLista.lista = pedidoLista.lista 
                   .GroupBy(x => x.ID)
                   .Select(g => new Itens
                   {
                       ID = g.Key,
                       Quantidade = g.Sum(x => x.Quantidade)
                   })
                   .ToList();

                List<PedidoItem> itens = new();

                foreach (var item in pedidoLista.lista)
                {
                    Produto produto =
                        await _repository.ObterProduto(item.ID);

                    //ValidarProduto(produto, item);

                    produto.EstoqueDisponivel -= item.Quantidade;

                    decimal valorTotal =
                        item.Quantidade * produto.Preco;

                    itens.Add(new PedidoItem
                    {
                        PrecoUnitario = produto.Preco,
                        Quantidade = item.Quantidade,                     
                        ValorTotal = valorTotal,
                        ProdutoId = produto.ProdutoId,
                        UpdatedAt = DateTimeHelper.ToSaoPaulo(),
                        CreatedAt = DateTimeHelper.ToSaoPaulo()
                    });

                    pedido.ValorTotal += valorTotal;
                }

                pedido.ClienteId = cliente.ClienteId;

                pedido.PedidoStatusId = (int)StatusPedido.Criado;

                pedido.CreatedAt =
                    DateTimeHelper.ToSaoPaulo();

                await _repository.CriarPedido(
                    pedido,
                    itens);

                await _repository.AdicionarHistorico(pedido);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<PedidosGetAllResponse> Listar(string query, int quantidade, int page)
        {
            if (quantidade > 20 || quantidade < 1)
                throw new CustomException("Quantidade inválida", HttpStatusCode.BadRequest);

            if (page < 1)
                throw new CustomException("Página inválida", HttpStatusCode.BadRequest);

            var result = await _repository.Listar(query, quantidade, page);

            if (result.Pedidos.Count == 0)
                throw new CustomException("Nenhum Resultado Encontrado", HttpStatusCode.NotFound);

            return result;
        }

        public async Task AlterarStatus(Guid pedido, StatusPedido novoStatus, string? motivo = null)
        {
            await AlterarStatus(await _repository.ObterPedido(pedido), novoStatus, motivo);
        }

        public async Task AlterarStatus(Pedido pedido, StatusPedido novoStatus, string? motivo = null)
        {
            StatusPedido status = (StatusPedido)pedido.PedidoStatusId;

            bool transicaoValida = status switch
            {
                StatusPedido.Criado =>
                    novoStatus == StatusPedido.Pago ||
                    novoStatus == StatusPedido.Cancelado,

                StatusPedido.Pago =>
                    novoStatus == StatusPedido.Enviado,

                _ => false
            };

            if (!transicaoValida)
                throw new CustomException($"Transição inválida: {status} -> {novoStatus}", HttpStatusCode.Unauthorized);

            await _repository.AlterarStatus(pedido, novoStatus);
            await _repository.AdicionarHistorico(pedido, motivo);
        }
    }
}
