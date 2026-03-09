using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Financeiro;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AnexoLancamentoConfiguration : IEntityTypeConfiguration<AnexoLancamento>
{
    public void Configure(EntityTypeBuilder<AnexoLancamento> b)
    {
        b.ToTable("AnexosLancamento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.Url).HasMaxLength(1000).IsRequired();
        b.Property(e => e.TipoArquivo).HasMaxLength(50).IsRequired();
        b.HasOne(e => e.Lancamento).WithMany(l => l.Anexos).HasForeignKey(e => e.LancamentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
