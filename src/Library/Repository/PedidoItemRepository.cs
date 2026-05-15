using EFCore;
using Library.ResponseDTO;
using Library.UTIL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Library.Repository
{
    public class PedidoItemRepository : IPedidoItemRepository
    {
        private readonly ContextEFCore _context;

        public PedidoItemRepository(ContextEFCore context)
        {
            _context = context;
        }

        public async Task<List<PedidoItemList>> ObterProdutos(int pedidoId)
        {
            List<PedidoItemList> itens = await _context.PedidoItem
            .Where(x => x.PedidoId == pedidoId)
            .Select(x => new PedidoItemList
            {
                Nome = x.Produto.Nome,
                Valor = x.PrecoUnitario,
                Quantidade = x.Quantidade,
                Total = x.ValorTotal
            })
            .ToListAsync();

            if (itens.Count == 0)
                throw new CustomException("Itens Não encontrados", HttpStatusCode.InternalServerError);

            return itens;
        }
    }
}
