using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.DTO
{
    public class ProdutoRequest
    {
        [MaxLength(200)]
        public required string Nome { get; set; } = null!;
        [MaxLength(1000)]
        public required string Descricao { get; set; } = null!;

        public required decimal Preco { get; set; }

        public int Estoque { get; set; } = 0;
    }
}
