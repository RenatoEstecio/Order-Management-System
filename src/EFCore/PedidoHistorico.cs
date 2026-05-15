using System;
using System.Collections.Generic;

namespace EFCore;

public partial class PedidoHistorico
{
    public int PedidoHistoricoId { get; set; }

    public int PedidoId { get; set; }

    public int PedidoStatusId { get; set; }

    public string? Motivo { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual PedidoStatus PedidoStatus { get; set; } = null!;
}
