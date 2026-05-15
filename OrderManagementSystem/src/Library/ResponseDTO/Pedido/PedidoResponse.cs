using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Pedido
{
    public class PedidoResponse 
    {
        public Guid Id { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }

        public string ClienteNome { get; set; } = string.Empty;
    }
}
