using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class PedidoItemList
    {
        public string Nome { get; set; }

        public decimal Valor { get; set; }

        public int Quantidade { get; set; }

        public decimal Total { get; set; }
    }
}
