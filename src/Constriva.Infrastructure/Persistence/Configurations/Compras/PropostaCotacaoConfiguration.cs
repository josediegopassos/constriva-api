using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class PropostaCotacaoConfiguration : IEntityTypeConfiguration<PropostaCotacao>
{
    public void Configure(EntityTypeBuilder<PropostaCotacao> b)
    {
        b.ToTable("PropostasCotacao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.CondicoesPagamento).HasMaxLength(300);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.ValorTotal).HasPrecision(18, 2);
        b.HasOne(e => e.Cotacao).WithMany(c => c.Propostas).HasForeignKey(e => e.CotacaoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Fornecedor).WithMany(f => f.Propostas).HasForeignKey(e => e.FornecedorId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Itens).WithOne(i => i.Proposta).HasForeignKey(i => i.PropostaId).OnDelete(DeleteBehavior.Cascade);
    }
}
