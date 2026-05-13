using EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class ProdutoListResponse
    {
        public string Id { get; set; }

        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }
                 

        public ProdutoListResponse(Produto produto)
        {
            Id = produto.Id;
            Nome = produto.Nome;
            Preco = produto.Preco;         
        }

        public ProdutoListResponse(){}
    }
}
