using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RevisaoDocumentoConfiguration : IEntityTypeConfiguration<RevisaoDocumento>
{
    public void Configure(EntityTypeBuilder<RevisaoDocumento> b)
    {
        b.ToTable("RevisoesDocumento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(1000);
        b.Property(e => e.Motivo).HasMaxLength(500).IsRequired();
        b.Property(e => e.ArquivoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Documento).WithMany(d => d.Revisoes).HasForeignKey(e => e.DocumentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
