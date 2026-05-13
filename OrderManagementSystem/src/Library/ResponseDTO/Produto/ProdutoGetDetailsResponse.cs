using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class ProdutoGetDetailsResponse : ResponseBase
    {
        public ProdutoDetailsResponse? Produto { get; set; }
    }
}
