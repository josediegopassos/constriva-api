using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Contratos;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ContratoConfiguration : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> b)
    {
        b.ToTable("Contratos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(50).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Objeto).HasMaxLength(500).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.ValorGlobal).HasPrecision(18, 2);
        b.Property(e => e.ValorAditivos).HasPrecision(18, 2);
        b.Property(e => e.ValorMedidoAcumulado).HasPrecision(18, 2);
        b.Property(e => e.ValorPagoAcumulado).HasPrecision(18, 2);
        b.Property(e => e.PercentualRetencao).HasPrecision(5, 2);
        b.Property(e => e.ValorRetencao).HasPrecision(18, 2);
        b.Ignore(e => e.ValorTotal);
        b.Property(e => e.CondicoesPagamento).HasMaxLength(300);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.ArquivoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Fornecedor).WithMany(f => f.Contratos).HasForeignKey(e => e.FornecedorId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Aditivos).WithOne(a => a.Contrato).HasForeignKey(a => a.ContratoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Medicoes).WithOne(m => m.Contrato).HasForeignKey(m => m.ContratoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Faturas).WithOne(f => f.Contrato).HasForeignKey(f => f.ContratoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Anexos).WithOne(a => a.Contrato).HasForeignKey(a => a.ContratoId).OnDelete(DeleteBehavior.Cascade);
    }
}
