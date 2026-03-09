using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemPedidoCompraConfiguration : IEntityTypeConfiguration<ItemPedidoCompra>
{
    public void Configure(EntityTypeBuilder<ItemPedidoCompra> b)
    {
        b.ToTable("ItensPedidoCompra");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.QuantidadePedida).HasPrecision(18, 4);
        b.Property(e => e.QuantidadeRecebida).HasPrecision(18, 4);
        b.Ignore(e => e.QuantidadePendente);
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Ignore(e => e.ValorTotal);
        b.HasOne(e => e.Pedido).WithMany(p => p.Itens).HasForeignKey(e => e.PedidoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
