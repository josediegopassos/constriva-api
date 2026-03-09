using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AcaoCorretivaSstConfiguration : IEntityTypeConfiguration<AcaoCorretivaSst>
{
    public void Configure(EntityTypeBuilder<AcaoCorretivaSst> b)
    {
        b.ToTable("AcoesCorretivasSST");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(1000).IsRequired();
        b.Property(e => e.Evidencia).HasMaxLength(1000);
        b.HasOne(e => e.Acidente).WithMany(a => a.AcoesCorretivas).HasForeignKey(e => e.AcidenteId).OnDelete(DeleteBehavior.Cascade);
    }
}
