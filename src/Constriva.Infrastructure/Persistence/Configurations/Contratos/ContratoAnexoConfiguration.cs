using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Contratos;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ContratoAnexoConfiguration : IEntityTypeConfiguration<ContratoAnexo>
{
    public void Configure(EntityTypeBuilder<ContratoAnexo> b)
    {
        b.ToTable("ContratoAnexos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.TipoArquivo).HasMaxLength(50).IsRequired();
        b.Property(e => e.Url).HasMaxLength(1000).IsRequired();
        b.HasOne(e => e.Contrato).WithMany(c => c.Anexos).HasForeignKey(e => e.ContratoId).OnDelete(DeleteBehavior.Cascade);
    }
}
