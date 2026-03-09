using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Financeiro;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class OrcamentoDREConfiguration : IEntityTypeConfiguration<OrcamentoDRE>
{
    public void Configure(EntityTypeBuilder<OrcamentoDRE> b)
    {
        b.ToTable("OrcamentosDRE");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Competencia).HasMaxLength(7).IsRequired();
        b.Property(e => e.Receitas).HasPrecision(18, 2);
        b.Property(e => e.Deducoes).HasPrecision(18, 2);
        b.Ignore(e => e.ReceitaLiquida);
        b.Property(e => e.CustosDiretos).HasPrecision(18, 2);
        b.Ignore(e => e.LucroBruto);
        b.Property(e => e.DespesasAdministrativas).HasPrecision(18, 2);
        b.Property(e => e.DespesasComerciais).HasPrecision(18, 2);
        b.Property(e => e.DespesasFinanceiras).HasPrecision(18, 2);
        b.Ignore(e => e.ResultadoOperacional);
        b.Ignore(e => e.EBITDA);
        b.Property(e => e.IRCSLL).HasPrecision(18, 2);
        b.Ignore(e => e.LucroLiquido);
    }
}
