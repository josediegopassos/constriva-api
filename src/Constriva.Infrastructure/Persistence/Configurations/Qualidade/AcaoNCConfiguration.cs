using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AcaoNCConfiguration : IEntityTypeConfiguration<AcaoNC>
{
    public void Configure(EntityTypeBuilder<AcaoNC> b)
    {
        b.ToTable("AcoesNC");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(1000).IsRequired();
        b.Property(e => e.Tipo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Evidencia).HasMaxLength(1000);
        b.HasOne(e => e.NaoConformidade).WithMany(nc => nc.Acoes).HasForeignKey(e => e.NaoConformidadeId).OnDelete(DeleteBehavior.Cascade);
    }
}
