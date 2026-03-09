using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ComposicaoOrcamentoConfiguration : IEntityTypeConfiguration<ComposicaoOrcamento>
{
    public void Configure(EntityTypeBuilder<ComposicaoOrcamento> b)
    {
        b.ToTable("ComposicoesOrcamento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.Fonte).HasConversion<int>();
        b.Property(e => e.CodigoFonte).HasMaxLength(30);
        b.Property(e => e.CustoTotal).HasPrecision(18, 4);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Insumos).WithOne(i => i.Composicao).HasForeignKey(i => i.ComposicaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
