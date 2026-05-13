using EFCore;
using Library.DTO;
using Library.Enums;
using Library.Repository;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Library.BLL
{
    public class PedidoService
    {
        private readonly PedidoRepository _repository;

        public PedidoService(PedidoRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecutarPedido(
            Cliente cliente,
            ListaPedidoRequest pedidoLista)
        {
            await using var transaction =
                await _repository.BeginTransaction();

            try
            {
                Pedido pedido = new();

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
                        CreatedAt = DateTimeHelper.ToSaoPaulo()
                    });

                    pedido.ValorTotal += valorTotal;
                }

                pedido.ClienteId = cliente.ClienteId;

                pedido.PedidoStatusId =
                    (int)StatusPedido.Criado;

                pedido.CreatedAt =
                    DateTimeHelper.ToSaoPaulo();

                await _repository.CriarPedido(
                    pedido,
                    itens);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
