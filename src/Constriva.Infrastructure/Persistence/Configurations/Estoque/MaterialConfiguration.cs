using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> b)
    {
        b.ToTable("Materiais");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(50).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique().HasFilter("\"IsDeleted\" = false");
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Especificacao).HasMaxLength(2000);
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.CodigoBarras).HasMaxLength(50);
        b.Property(e => e.CodigoSINAPI).HasMaxLength(20);
        b.Property(e => e.Marca).HasMaxLength(100);
        b.Property(e => e.Fabricante).HasMaxLength(200);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.EstoqueMinimo).HasPrecision(18, 4);
        b.Property(e => e.EstoqueMaximo).HasPrecision(18, 4);
        b.Property(e => e.PrecoCustoMedio).HasPrecision(18, 4);
        b.Property(e => e.PrecoUltimaCompra).HasPrecision(18, 4);
        b.Property(e => e.ImagemUrl).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
    }
}
