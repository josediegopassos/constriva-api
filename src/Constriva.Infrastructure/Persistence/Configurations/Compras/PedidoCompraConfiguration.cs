using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class PedidoCompraConfiguration : IEntityTypeConfiguration<PedidoCompra>
{
    public void Configure(EntityTypeBuilder<PedidoCompra> b)
    {
        b.ToTable("PedidosCompra");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.CondicoesPagamento).HasMaxLength(300);
        b.Property(e => e.LocalEntrega).HasMaxLength(300);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.ValorFrete).HasPrecision(18, 2);
        b.Property(e => e.ValorDesconto).HasPrecision(18, 2);
        b.Property(e => e.ValorTotal).HasPrecision(18, 2);
        b.Property(e => e.MotivoRejeicao).HasMaxLength(500);
        b.HasOne(e => e.Fornecedor).WithMany(f => f.PedidosCompra).HasForeignKey(e => e.FornecedorId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Recebimentos).WithOne(r => r.Pedido).HasForeignKey(r => r.PedidoId).OnDelete(DeleteBehavior.Cascade);
    }
}
