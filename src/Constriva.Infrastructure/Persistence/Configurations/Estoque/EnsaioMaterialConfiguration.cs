using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class EnsaioMaterialConfiguration : IEntityTypeConfiguration<EnsaioMaterial>
{
    public void Configure(EntityTypeBuilder<EnsaioMaterial> b)
    {
        b.ToTable("EnsaiosMaterial");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.Property(e => e.Laboratorio).HasMaxLength(200);
        b.Property(e => e.NormaReferencia).HasMaxLength(100);
        b.Property(e => e.Resultado).HasMaxLength(100);
        b.Property(e => e.ValorObtido).HasPrecision(18, 4);
        b.Property(e => e.ValorMinimo).HasPrecision(18, 4);
        b.Property(e => e.ValorMaximo).HasPrecision(18, 4);
        b.Property(e => e.Laudo).HasMaxLength(2000);
        b.Property(e => e.LaudoUrl).HasMaxLength(1000);
        b.Property(e => e.LocalColeta).HasMaxLength(200);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
    }
}
