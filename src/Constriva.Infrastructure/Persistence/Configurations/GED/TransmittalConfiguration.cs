using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class TransmittalConfiguration : IEntityTypeConfiguration<Transmittal>
{
    public void Configure(EntityTypeBuilder<Transmittal> b)
    {
        b.ToTable("Transmittals");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Assunto).HasMaxLength(300).IsRequired();
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.RemetNome).HasMaxLength(200);
        b.Property(e => e.DestNome).HasMaxLength(200);
        b.Property(e => e.DestEmail).HasMaxLength(200);
        b.HasMany(e => e.Documentos).WithOne(d => d.Transmittal).HasForeignKey(d => d.TransmittalId).OnDelete(DeleteBehavior.Cascade);
    }
}
