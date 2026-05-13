using System;
using System.Collections.Generic;

namespace EFCore;

public partial class Pedido
{
    public int PedidoId { get; set; }

    public Guid Id { get; set; }

    public int ClienteId { get; set; }

    public int PedidoStatusId { get; set; }

    public decimal ValorTotal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<PedidoHistorico> PedidoHistorico { get; set; } = new List<PedidoHistorico>();

    public virtual ICollection<PedidoItem> PedidoItem { get; set; } = new List<PedidoItem>();

    public virtual PedidoStatus PedidoStatus { get; set; } = null!;
}
