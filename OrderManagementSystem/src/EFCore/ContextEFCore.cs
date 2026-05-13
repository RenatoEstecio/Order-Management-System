using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFCore;

public partial class ContextEFCore : DbContext
{
    public ContextEFCore()
    {
    }

    public ContextEFCore(DbContextOptions<ContextEFCore> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<PedidoItem> PedidoItem { get; set; }

    public virtual DbSet<Produto> Produto { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=Oms_bd;Persist Security Info=False;User ID=sa;Password=Oms1600@SQL;MultipleActiveResultSets=False;Encrypt=True;Max Pool Size=1500;TrustServerCertificate=True;Connection Timeout=300;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__71ABD087EEFA52A4");

            entity.HasIndex(e => e.Id, "UQ__Cliente__3214EC06E81E5759").IsUnique();

            entity.HasIndex(e => e.Documento, "UQ__Cliente__AF73706D91E137A6").IsUnique();

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Documento).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nome).HasMaxLength(200);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidoId).HasName("PK__Pedido__09BA14304658EFCF");

            entity.HasIndex(e => e.Id, "UQ__Pedido__3214EC0668DB3487").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ValorTotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Clientes");
        });

        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.HasKey(e => e.PedidoItemId).HasName("PK__PedidoIt__4A8A527353DE8A65");

            entity.Property(e => e.PrecoUnitario).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ValorTotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoItem)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoItens_Pedidos");

            entity.HasOne(d => d.Produto).WithMany(p => p.PedidoItem)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoItens_Produtos");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.ProdutoId).HasName("PK__Produto__9C8800E315B8250D");

            entity.HasIndex(e => e.Id, "UQ__Produto__3213E83E6ECAAD86").IsUnique();

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Descricao).HasMaxLength(1000);
            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .HasDefaultValueSql("('P-'+left(CONVERT([varchar](36),newid()),(4)))")
                .HasColumnName("id");
            entity.Property(e => e.Nome).HasMaxLength(200);
            entity.Property(e => e.Preco).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
