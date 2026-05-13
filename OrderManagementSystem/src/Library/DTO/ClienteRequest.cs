using EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.DTO
{
    public class ClienteRequest
    {       
        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Documento { get; set; } = null!;            
    }
}
