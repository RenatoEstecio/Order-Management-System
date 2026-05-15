using EFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.DTO
{
    public class ClienteRequest
    {
        [MaxLength(200)]
        public string Nome { get; set; } = null!;
        [MaxLength(200)]
        public string Email { get; set; } = null!;
        [MaxLength(20)]
        public string Documento { get; set; } = null!;            
    }
}
