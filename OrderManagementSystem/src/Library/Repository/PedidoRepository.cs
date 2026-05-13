using EFCore;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Library.Repository
{
    public class PedidoRepository
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
                throw new CustomException(
                    "Produto não encontrado");

            return produto;
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
    }
}
