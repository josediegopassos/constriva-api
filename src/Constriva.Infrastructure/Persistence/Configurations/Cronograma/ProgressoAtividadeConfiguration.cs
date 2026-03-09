using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ProgressoAtividadeConfiguration : IEntityTypeConfiguration<ProgressoAtividade>
{
    public void Configure(EntityTypeBuilder<ProgressoAtividade> b)
    {
        b.ToTable("ProgressosAtividade");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.PercentualAnterior).HasPrecision(5, 2);
        b.Property(e => e.PercentualAtual).HasPrecision(5, 2);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Atividade).WithMany().HasForeignKey(e => e.AtividadeId).OnDelete(DeleteBehavior.Cascade);
    }
}
