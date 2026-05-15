using System;
using System.Collections.Generic;
using System.Text;

namespace Library.DTO
{
    public class ListaPedidoRequest 
    {
        public List<Itens> lista { get; set; } = [];
        public required Guid cliente { get; set; } 

    }
}
