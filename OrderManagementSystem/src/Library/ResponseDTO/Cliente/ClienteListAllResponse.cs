using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Clientes
{
    public class ClienteListAllResponse : ResponseBase
    {       
        public List<ClienteListResponse>? Clientes { get; set; }

        public int Pagina { get; set; } = 0;
        public int Resultados { get; set; } = 0;
        public int TotalPagina { get; set; } = 0;
    }
}
