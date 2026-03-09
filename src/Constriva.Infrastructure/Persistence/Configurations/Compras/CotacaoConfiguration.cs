using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class CotacaoConfiguration : IEntityTypeConfiguration<Cotacao>
{
    public void Configure(EntityTypeBuilder<Cotacao> b)
    {
        b.ToTable("Cotacoes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Titulo).HasMaxLength(300).IsRequired();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.CondicoesGerais).HasMaxLength(2000);
        b.HasMany(e => e.Itens).WithOne(i => i.Cotacao).HasForeignKey(i => i.CotacaoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Propostas).WithOne(p => p.Cotacao).HasForeignKey(p => p.CotacaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
