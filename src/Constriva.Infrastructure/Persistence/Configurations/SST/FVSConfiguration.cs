using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FVSConfiguration : IEntityTypeConfiguration<FVS>
{
    public void Configure(EntityTypeBuilder<FVS> b)
    {
        b.ToTable("FVS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Servico).HasMaxLength(300).IsRequired();
        b.Property(e => e.EtapaConstrucao).HasMaxLength(100);
        b.Property(e => e.Pavimento).HasMaxLength(50);
        b.Property(e => e.Area).HasMaxLength(100);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.AssinaturaUrl).HasMaxLength(1000);
        b.HasMany(e => e.Itens).WithOne(i => i.FVSVinculado).HasForeignKey(i => i.FVSId).OnDelete(DeleteBehavior.Cascade);
    }
}
