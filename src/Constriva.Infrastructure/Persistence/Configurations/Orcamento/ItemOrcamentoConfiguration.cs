using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemOrcamentoConfiguration : IEntityTypeConfiguration<ItemOrcamento>
{
    public void Configure(EntityTypeBuilder<ItemOrcamento> b)
    {
        b.ToTable("ItensOrcamento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.Fonte).HasConversion<int>();
        b.Property(e => e.CodigoFonte).HasMaxLength(30);
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.Quantidade).HasPrecision(18, 4);
        b.Property(e => e.CustoUnitario).HasPrecision(18, 4);
        b.Ignore(e => e.CustoTotal);
        b.Property(e => e.CustoComBDI).HasPrecision(18, 2);
        b.Property(e => e.BDI).HasPrecision(5, 2);
        b.HasOne(e => e.Grupo).WithMany(g => g.Itens).HasForeignKey(e => e.GrupoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Composicao).WithMany().HasForeignKey(e => e.ComposicaoId).OnDelete(DeleteBehavior.Restrict);
    }
}
