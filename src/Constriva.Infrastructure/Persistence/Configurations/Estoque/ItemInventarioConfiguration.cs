using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemInventarioConfiguration : IEntityTypeConfiguration<ItemInventario>
{
    public void Configure(EntityTypeBuilder<ItemInventario> b)
    {
        b.ToTable("ItensInventario");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.SaldoSistema).HasPrecision(18, 4);
        b.Property(e => e.SaldoContado).HasPrecision(18, 4);
        b.Property(e => e.CustoUnitario).HasPrecision(18, 4);
        b.Ignore(e => e.Diferenca);
        b.Ignore(e => e.ValorDiferenca);
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.HasOne(e => e.Inventario).WithMany(i => i.Itens).HasForeignKey(e => e.InventarioId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
