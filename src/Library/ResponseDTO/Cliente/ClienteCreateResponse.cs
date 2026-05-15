
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Clientes
{
    public class ClienteCreateResponse : ResponseBase
    {      
        public ClienteDetailsResponse? Cliente { get; set; }
    }
}
