using Azure.Core;
using EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Library.DTO;
using Library.UTIL;

namespace Library.BLL
{
    public class PedidoBLL 
    {
      
        Cliente cliente;
        private readonly ContextEFCore _context;

        public PedidoBLL(Cliente cliente) 
        { 
            this.cliente = cliente;
            _context = new ContextEFCore();
        }

        public async Task<bool> ExecutaPedido(ListaPedidoRequest pedidoLista)
        {          
            using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                Pedido pedido = new Pedido();
                List<PedidoItem> itens = new List<PedidoItem>();
                ProdutoBLL produtoBLL = new ProdutoBLL(_context);

                foreach (var item in pedidoLista.lista)
                {
                    Produto? produto = await AdicionarNaLista(item);

                    produto.EstoqueDisponivel -= item.Quantidade;

                    _context.Entry(produto)
                        .Property(x => x.EstoqueDisponivel)
                        .IsModified = true;

                    PedidoItem _itens = new PedidoItem();

                    decimal valorTotal = item.Quantidade * produto.Preco;

                    _itens.PrecoUnitario = produto.Preco;
                    _itens.Quantidade = item.Quantidade;
                    _itens.ValorTotal = valorTotal;

                    itens.Add(_itens);

                    pedido.ValorTotal += valorTotal;
                }
               
                pedido.Status = (int)StatusPedido.Criado;

                pedido.ClienteId = cliente.ClienteId;

                pedido.CreatedAt = DateTimeHelper.ToSaoPaulo();

                _context.Pedido.Add(pedido);

                foreach (var i in itens)
                {
                    i.PedidoId = pedido.PedidoId;
                    i.CreatedAt = DateTimeHelper.ToSaoPaulo();

                    _context.PedidoItem.Add(i);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        async Task<Produto> AdicionarNaLista(Itens item)
        {
            Produto? produto = await _context.Produto
                        .FirstOrDefaultAsync(x =>
                            x.Id == item.ID);

            if (produto is null)
                throw new Exception("Produto não encontrado");

            if (!produto.Ativo)
                throw new Exception("Produto inativo");

            if (produto.EstoqueDisponivel < item.Quantidade)
                throw new Exception(
                    $"Estoque insuficiente para {produto.Nome}");

            return produto;
        }


        public enum StatusPedido
        {
            Criado = 1,
            Pago = 2,
            Enviado = 3,
            Finalizado = 4,
            Cancelado = 5
        }
    }
}
