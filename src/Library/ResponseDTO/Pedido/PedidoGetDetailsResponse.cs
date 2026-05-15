using EFCore;
using Library.ResponseDTO.Clientes;

using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class PedidoGetDetailsResponse : ResponseBase
    {
        public Guid Id { get; set; }

        public ClienteListResponse Cliente { get; set; }

        public List<HistoricoListResponse> Historico { get; set; } = new List<HistoricoListResponse>();

        public List<PedidoItemList> Produtos { get; set; }

        public string Status { get; set; } 
        public decimal Total { get; set; }
    
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }       
    }
}
