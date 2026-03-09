using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ArquivoDocumentoConfiguration : IEntityTypeConfiguration<ArquivoDocumento>
{
    public void Configure(EntityTypeBuilder<ArquivoDocumento> b)
    {
        b.ToTable("ArquivosDocumento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.NomeArquivo).HasMaxLength(300).IsRequired();
        b.Property(e => e.TipoArquivo).HasMaxLength(50).IsRequired();
        b.Property(e => e.Url).HasMaxLength(1000).IsRequired();
        b.Property(e => e.HashArquivo).HasMaxLength(100);
        b.HasOne(e => e.Documento).WithMany(d => d.Arquivos).HasForeignKey(e => e.DocumentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
