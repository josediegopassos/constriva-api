using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DocumentoPCMConfiguration : IEntityTypeConfiguration<DocumentoPCM>
{
    public void Configure(EntityTypeBuilder<DocumentoPCM> b)
    {
        b.ToTable("DocumentosPCM");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.TipoDocumento).HasMaxLength(20).IsRequired();
        b.Property(e => e.NumeroRevisao).HasMaxLength(10);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Elaborador).HasMaxLength(200);
        b.Property(e => e.AprovadoPor).HasMaxLength(200);
        b.Property(e => e.ArquivoUrl).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
    }
}
