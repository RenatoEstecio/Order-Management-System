using System;
using System.Collections.Generic;
using System.Text;

namespace Library.DTO
{
    public class ProdutoRequest
    {
        public required string Nome { get; set; } = null!;

        public required string Descricao { get; set; } = null!;

        public required decimal Preco { get; set; }

        public int Estoque { get; set; } = 0;
    }
}
