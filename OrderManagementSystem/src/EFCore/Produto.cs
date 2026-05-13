using System;
using System.Collections.Generic;

namespace EFCore;

public partial class Produto
{
    public int ProdutoId { get; set; }

    public string? Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int EstoqueDisponivel { get; set; }

    public bool Ativo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<PedidoItem> PedidoItem { get; set; } = new List<PedidoItem>();
}
