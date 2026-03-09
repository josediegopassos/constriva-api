using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemRecebimentoConfiguration : IEntityTypeConfiguration<ItemRecebimento>
{
    public void Configure(EntityTypeBuilder<ItemRecebimento> b)
    {
        b.ToTable("ItensRecebimento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.QuantidadeRecebida).HasPrecision(18, 4);
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Property(e => e.Lote).HasMaxLength(50);
        b.Property(e => e.MotivoReprovacao).HasMaxLength(500);
        b.HasOne(e => e.Recebimento).WithMany(r => r.Itens).HasForeignKey(e => e.RecebimentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
