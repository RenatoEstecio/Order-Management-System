using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.RequestDTO
{
    public class ProdutoUpdateRequest
    {
        [MaxLength(200)]
        public string? Nome { get; set; }
        [MaxLength(1000)]
        public string? Descricao { get; set; }     
    }
}
