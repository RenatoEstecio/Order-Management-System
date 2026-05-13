using EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO.Clientess
{
    public class ClienteDetailsResponse 
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Documento { get; set; } = null!;

        public bool Ativo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ClienteDetailsResponse(Cliente cliente) 
        {
            Id = cliente.Id;
            Nome = cliente.Nome;
            Email = cliente.Email;
            Documento = cliente.Documento;
            Ativo = cliente.Ativo;
            CreatedAt = cliente.CreatedAt;
            UpdatedAt = cliente.UpdatedAt;
        }
    }
}
