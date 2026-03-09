using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ObraAnexoConfiguration : IEntityTypeConfiguration<ObraAnexo>
{
    public void Configure(EntityTypeBuilder<ObraAnexo> b)
    {
        b.ToTable("ObraAnexos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.TipoArquivo).HasMaxLength(50).IsRequired();
        b.Property(e => e.Url).HasMaxLength(1000).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
    }
}
