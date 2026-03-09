using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class InventarioEstoqueConfiguration : IEntityTypeConfiguration<InventarioEstoque>
{
    public void Configure(EntityTypeBuilder<InventarioEstoque> b)
    {
        b.ToTable("InventariosEstoque");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.Property(e => e.Status).HasMaxLength(30).IsRequired();
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.ValorDiferenca).HasPrecision(18, 2);
        b.HasMany(e => e.Itens).WithOne(i => i.Inventario).HasForeignKey(i => i.InventarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
