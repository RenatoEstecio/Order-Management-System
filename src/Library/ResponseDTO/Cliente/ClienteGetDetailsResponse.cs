
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Clientes
{
    public class ClienteGetDetailsResponse : ResponseBase
    {
        public ClienteDetailsResponse? Cliente { get; set; }
    }
}
