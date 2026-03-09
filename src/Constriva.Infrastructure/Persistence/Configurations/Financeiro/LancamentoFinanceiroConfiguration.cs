using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Financeiro;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class LancamentoFinanceiroConfiguration : IEntityTypeConfiguration<LancamentoFinanceiro>
{
    public void Configure(EntityTypeBuilder<LancamentoFinanceiro> b)
    {
        b.ToTable("LancamentosFinanceiros");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Valor).HasPrecision(18, 2);
        b.Property(e => e.ValorRealizado).HasPrecision(18, 2);
        b.Property(e => e.FormaPagamentoEnum).HasConversion<int>();
        b.Property(e => e.StatusAprovacao).HasConversion<int>();
        b.Property(e => e.NumeroDocumento).HasMaxLength(50);
        b.Property(e => e.NumeroNF).HasMaxLength(50);
        b.Property(e => e.Periodicidade).HasMaxLength(30);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.ComprovantePagUrl).HasMaxLength(1000);
        b.HasIndex(e => new { e.EmpresaId, e.DataVencimento });
        b.HasIndex(e => new { e.EmpresaId, e.Status });
        b.HasOne(e => e.CentroCusto).WithMany(c => c.Lancamentos).HasForeignKey(e => e.CentroCustoId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.ContaBancaria).WithMany(cb => cb.Lancamentos).HasForeignKey(e => e.ContaBancariaId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.PlanoContas).WithMany().HasForeignKey(e => e.PlanoContaId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Anexos).WithOne(a => a.Lancamento).HasForeignKey(a => a.LancamentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
