using System;
using System.Collections.Generic;

namespace EFCore;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public bool Ativo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Pedido> Pedido { get; set; } = new List<Pedido>();
}
