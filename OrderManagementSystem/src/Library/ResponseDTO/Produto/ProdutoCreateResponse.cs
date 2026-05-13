using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ResponseDTO
{
    public class ProdutoCreateResponse : ResponseBase
    {      
        public ProdutoDetailsResponse? Produto { get; set; }
    }
}
