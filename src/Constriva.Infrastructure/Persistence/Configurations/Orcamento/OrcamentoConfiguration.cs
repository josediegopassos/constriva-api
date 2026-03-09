using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class OrcamentoConfiguration : IEntityTypeConfiguration<Orcamento>
{
    public void Configure(EntityTypeBuilder<Orcamento> b)
    {
        b.ToTable("Orcamentos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.BaseOrcamentaria).HasMaxLength(100);
        b.Property(e => e.Localidade).HasMaxLength(200);
        b.Property(e => e.BDI).HasPrecision(5, 2);
        b.Property(e => e.BDIDetalhado_Administracao).HasPrecision(5, 2);
        b.Property(e => e.BDIDetalhado_Seguro).HasPrecision(5, 2);
        b.Property(e => e.BDIDetalhado_Risco).HasPrecision(5, 2);
        b.Property(e => e.BDIDetalhado_Financeiro).HasPrecision(5, 2);
        b.Property(e => e.BDIDetalhado_Lucro).HasPrecision(5, 2);
        b.Property(e => e.BDIDetalhado_Tributos).HasPrecision(5, 2);
        b.Property(e => e.EncargosHoristas).HasPrecision(5, 2);
        b.Property(e => e.EncargosMensalistas).HasPrecision(5, 2);
        b.Property(e => e.ValorCustoDirecto).HasPrecision(18, 2);
        b.Property(e => e.ValorBDI).HasPrecision(18, 2);
        b.Property(e => e.ValorTotal).HasPrecision(18, 2);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Grupos).WithOne(g => g.Orcamento).HasForeignKey(g => g.OrcamentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Composicoes).WithOne().HasForeignKey(c => c.OrcamentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Historicos).WithOne(h => h.Orcamento).HasForeignKey(h => h.OrcamentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
