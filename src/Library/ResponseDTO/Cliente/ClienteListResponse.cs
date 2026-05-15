using EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Clientes
{
    public class ClienteListResponse
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;
                 

        public ClienteListResponse(Cliente cliente)
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Email = cliente.Email;         
        }

        public ClienteListResponse(){}
    }
}
