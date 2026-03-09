using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RecebimentoCompraConfiguration : IEntityTypeConfiguration<RecebimentoCompra>
{
    public void Configure(EntityTypeBuilder<RecebimentoCompra> b)
    {
        b.ToTable("RecebimentosCompra");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.NumeroNF).HasMaxLength(50);
        b.Property(e => e.SerieNF).HasMaxLength(10);
        b.Property(e => e.ValorNF).HasPrecision(18, 2);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasOne(e => e.Pedido).WithMany(p => p.Recebimentos).HasForeignKey(e => e.PedidoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Itens).WithOne(i => i.Recebimento).HasForeignKey(i => i.RecebimentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
