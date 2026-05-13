using EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class ProdutoDetailsResponse 
    {
        public string? Id { get; set; }

        public string Nome { get; set; } = null!;

        public string? Descricao { get; set; }

        public decimal Preco { get; set; }

        public int EstoqueDisponivel { get; set; }

        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ProdutoDetailsResponse(Produto produto) 
        {
            Id = produto.Id;
            Nome = produto.Nome;
            Descricao = produto.Descricao;
            Preco = produto.Preco;
            Ativo = produto.Ativo;
            CreatedAt = produto.CreatedAt;
            UpdatedAt = produto.UpdatedAt;
        }
    }
}
