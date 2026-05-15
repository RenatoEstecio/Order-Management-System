using EFCore;
using Library.Enums;
using Library.ResponseDTO.Pedido;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace Library.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ContextEFCore _context;

        public PedidoRepository(ContextEFCore context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction>
            BeginTransaction()
        {
            return await _context.Database
                .BeginTransactionAsync(
                    IsolationLevel.Serializable);
        }

        public async Task<Produto> ObterProduto(string id)
        {
            Produto? produto = await _context.Produto
                .FirstOrDefaultAsync(x => x.Id == id);

            if (produto is null)
                throw new CustomException("Produto não encontrado", HttpStatusCode.NotFound);

            return produto;
        }

        public async Task<Pedido> ObterPedido(Guid id)
        {
            Pedido? pedido = await _context.Pedido
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pedido is null)
                throw new CustomException("Pedido não encontrado", HttpStatusCode.NotFound);

            return pedido;
        }

        public async Task<PedidosGetAllResponse> Listar(string? query, int quantidade, int? page)
        {
            var consulta = _context.Pedido
                .AsNoTracking()
                .AsQueryable();

            PedidosGetAllResponse response = new();

            if (!string.IsNullOrWhiteSpace(query))
            {
                consulta = consulta.Where(x =>
                    x.Cliente.Nome.Contains(query) ||
                    x.PedidoStatus.Nome.Contains(query) ||
                    x.Id.ToString().Contains(query));
            }

            var totalItems = await consulta.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / quantidade);

            var paginaAtual = page ?? 1;

            var skip = (paginaAtual - 1) * quantidade;

            var result = await consulta
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(quantidade)
                .Select(x => new PedidoResponse
                {
                    Id = x.Id,
                    Status = x.PedidoStatus.Nome,
                    UpdatedAt = x.UpdatedAt,
                    ClienteNome = x.Cliente.Nome
                })
                .ToListAsync();

            response.Resultados = totalItems;
            response.Pagina = paginaAtual;
            response.TotalPagina = totalPages;
            response.Pedidos = result;

            response.Message = "Sucesso";
            if (response.Pedidos.Count == 0)
                response.Message = "Nenhum item encontrado";
            else
                response.Message = "Sucesso";

            return response;
        }

        public async Task<PedidosGetAllResponse> ObterPedidos(Guid id)
        {
            PedidosGetAllResponse pedidos = new PedidosGetAllResponse();
            pedidos.Pedidos = await _context.Pedido
            .AsNoTracking()
            .Select(p => new PedidoResponse
            {
                Id = p.Id,
                Status = p.PedidoStatus.Nome,
                UpdatedAt = p.UpdatedAt,
                ClienteNome = p.Cliente.Nome
            })
            .ToListAsync();
            
            return pedidos;
        }

        public async Task CriarPedido(
            Pedido pedido,
            List<PedidoItem> itens)
        {
            await _context.Pedido.AddAsync(pedido);

            await _context.SaveChangesAsync();

            foreach (var item in itens)
            {
                item.PedidoId = pedido.PedidoId;
            }

            await _context.PedidoItem.AddRangeAsync(itens);

            await _context.SaveChangesAsync();
        }

        public async Task AdicionarHistorico(Pedido pedido, string? motivo = null)
        {
            PedidoHistorico pedidoHistorico = new PedidoHistorico();

            pedidoHistorico.PedidoId = pedido.PedidoId;
            pedidoHistorico.PedidoStatusId = pedido.PedidoStatusId;
            pedidoHistorico.CreatedAt = DateTimeHelper.ToSaoPaulo();

            if(!motivo.IsNullOrEmpty())
                pedidoHistorico.Motivo = motivo;

            await _context.PedidoHistorico.AddAsync(pedidoHistorico);

            await _context.SaveChangesAsync();
        }

        public async Task AlterarStatus(Pedido pedido, StatusPedido novoStatus)
        {
            pedido.PedidoStatusId = (int)novoStatus;

            var linhasAfetadas = await _context.Pedido
           .Where(x =>
               x.Id == pedido.Id &&
               (
                   (x.PedidoStatusId == (int)StatusPedido.Criado &&
                       (novoStatus == StatusPedido.Pago ||
                        novoStatus == StatusPedido.Cancelado))

                   ||

                   (x.PedidoStatusId == (int)StatusPedido.Pago &&
                       novoStatus == StatusPedido.Enviado)
               ))
           .ExecuteUpdateAsync(setters =>
               setters
                   .SetProperty(
                       x => x.PedidoStatusId,
                       (int)novoStatus)
                   .SetProperty(
                       x => x.UpdatedAt,
                       DateTimeHelper.ToSaoPaulo()));


            if(!(linhasAfetadas > 0))
                throw new CustomException("Alteração não autorizada",HttpStatusCode.Unauthorized);
        }
        
    }
}
