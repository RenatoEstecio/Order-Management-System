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
    public class PedidoHistoricoRepository
    {
        private readonly ContextEFCore _context;

        public PedidoHistoricoRepository(ContextEFCore context)
        {
            _context = context;
        }

        public async Task<List<PedidoHistorico>> ObterHistorico(int pedidoId)
        {
            List<PedidoHistorico> itens = await _context.PedidoHistorico
            .Where(x => x.PedidoId == pedidoId)        
            .ToListAsync();

           
            return itens;
        }
    }
}
