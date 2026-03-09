using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DocumentoTransmittalConfiguration : IEntityTypeConfiguration<DocumentoTransmittal>
{
    public void Configure(EntityTypeBuilder<DocumentoTransmittal> b)
    {
        b.ToTable("DocumentosTransmittal");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Finalidade).HasMaxLength(50);
        b.HasOne(e => e.Transmittal).WithMany(t => t.Documentos).HasForeignKey(e => e.TransmittalId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Documento).WithMany().HasForeignKey(e => e.DocumentoId).OnDelete(DeleteBehavior.Restrict);
    }
}
