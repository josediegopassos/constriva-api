using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class MovimentacaoEstoqueConfiguration : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> b)
    {
        b.ToTable("MovimentacoesEstoque");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.Quantidade).HasPrecision(18, 4);
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Property(e => e.SaldoAnterior).HasPrecision(18, 4);
        b.Property(e => e.SaldoPosterior).HasPrecision(18, 4);
        b.Ignore(e => e.ValorTotal);
        b.Property(e => e.NumeroDocumento).HasMaxLength(50);
        b.Property(e => e.NumeroNF).HasMaxLength(50);
        b.Property(e => e.Lote).HasMaxLength(50);
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.HasOne(e => e.Material).WithMany(m => m.Movimentacoes).HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Almoxarifado).WithMany(a => a.Movimentacoes).HasForeignKey(e => e.AlmoxarifadoId).OnDelete(DeleteBehavior.Restrict);
    }
}
