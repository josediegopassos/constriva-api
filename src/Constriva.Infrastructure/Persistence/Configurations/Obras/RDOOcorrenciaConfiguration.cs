using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras.Diario;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RDOOcorrenciaConfiguration : IEntityTypeConfiguration<RDOOcorrencia>
{
    public void Configure(EntityTypeBuilder<RDOOcorrencia> b)
    {
        b.ToTable("RDOOcorrencias");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(2000).IsRequired();
        b.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
        b.Property(e => e.Resolucao).HasMaxLength(2000);
        b.HasOne(e => e.RDO).WithMany(r => r.Ocorrencias).HasForeignKey(e => e.RDOId).OnDelete(DeleteBehavior.Cascade);
    }
}
