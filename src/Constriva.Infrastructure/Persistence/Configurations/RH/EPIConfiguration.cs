using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class EPIConfiguration : IEntityTypeConfiguration<EPI>
{
    public void Configure(EntityTypeBuilder<EPI> b)
    {
        b.ToTable("EPIs");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(1000);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Fabricante).HasMaxLength(200);
        b.Property(e => e.Modelo).HasMaxLength(100);
        b.Property(e => e.NumeroCA).HasMaxLength(20);
        b.Property(e => e.NormaReferencia).HasMaxLength(100);
        b.Property(e => e.VidaUtilMeses).HasPrecision(5, 1);
        b.Property(e => e.ImagemUrl).HasMaxLength(1000);
    }
}
