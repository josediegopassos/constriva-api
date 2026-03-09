using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemFVSConfiguration : IEntityTypeConfiguration<ItemFVS>
{
    public void Configure(EntityTypeBuilder<ItemFVS> b)
    {
        b.ToTable("ItensFVS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.Criterio).HasMaxLength(1000);
        b.Property(e => e.Resultado).HasMaxLength(20).IsRequired();
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.HasOne(e => e.FVSVinculado).WithMany(f => f.Itens).HasForeignKey(e => e.FVSId).OnDelete(DeleteBehavior.Cascade);
    }
}
