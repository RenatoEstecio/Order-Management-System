using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Pedido
{
    public class PedidosGetAllResponse : ResponseBase
    {
        public List<PedidoResponse> Pedidos {  get; set; }

        public int Pagina { get; set; } = 0;
        public int Resultados { get; set; } = 0;
        public int TotalPagina { get; set; } = 0;
    }
}
