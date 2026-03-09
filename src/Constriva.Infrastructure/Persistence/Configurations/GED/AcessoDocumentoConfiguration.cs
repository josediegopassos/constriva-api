using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AcessoDocumentoConfiguration : IEntityTypeConfiguration<AcessoDocumento>
{
    public void Configure(EntityTypeBuilder<AcessoDocumento> b)
    {
        b.ToTable("AcessosDocumento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.DocumentoId, e.UsuarioId }).IsUnique();
        b.HasOne(e => e.Documento).WithMany(d => d.Acessos).HasForeignKey(e => e.DocumentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
