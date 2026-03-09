using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class InsumoComposicaoConfiguration : IEntityTypeConfiguration<InsumoComposicao>
{
    public void Configure(EntityTypeBuilder<InsumoComposicao> b)
    {
        b.ToTable("InsumosComposicao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.Coeficiente).HasPrecision(18, 6);
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Ignore(e => e.CustoTotal);
        b.Property(e => e.FontePrecoEnum).HasConversion<int>();
        b.HasOne(e => e.Composicao).WithMany(c => c.Insumos).HasForeignKey(e => e.ComposicaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
