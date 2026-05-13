using System;
using System.Collections.Generic;

namespace EFCore;

public partial class PedidoIten
{
    public int PedidoItenId { get; set; }

    public int PedidoId { get; set; }

    public int ProdutoId { get; set; }

    public int Quantidade { get; set; }

    public decimal PrecoUnitario { get; set; }

    public decimal ValorTotal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Produto Produto { get; set; } = null!;
}
