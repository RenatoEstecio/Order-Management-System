using Library.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.RequestDTO
{
    public class PedidoAlteracaoRequest
    {
        public required StatusPedidoAlteracao alteracao { get; set; }
        [MaxLength(400)]
        public string? Motivo { get; set; } = null!;
    }
}
