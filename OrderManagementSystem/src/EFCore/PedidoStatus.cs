using System;
using System.Collections.Generic;

namespace EFCore;

public partial class PedidoStatus
{
    public int PedidoStatusId { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();

    public virtual ICollection<PedidoHistorico> PedidoHistorico { get; set; } = new List<PedidoHistorico>();
}
