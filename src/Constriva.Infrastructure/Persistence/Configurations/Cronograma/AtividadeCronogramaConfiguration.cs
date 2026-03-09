using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AtividadeCronogramaConfiguration : IEntityTypeConfiguration<AtividadeCronograma>
{
    public void Configure(EntityTypeBuilder<AtividadeCronograma> b)
    {
        b.ToTable("AtividadesCronograma");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.PercentualConcluido).HasPrecision(5, 2);
        b.Property(e => e.BCWS).HasPrecision(18, 2);
        b.Property(e => e.BCWP).HasPrecision(18, 2);
        b.Property(e => e.ACWP).HasPrecision(18, 2);
        b.Ignore(e => e.SV);
        b.Ignore(e => e.CV);
        b.Ignore(e => e.SPI);
        b.Ignore(e => e.CPI);
        b.Property(e => e.CustoOrcado).HasPrecision(18, 2);
        b.Property(e => e.CustoRealizado).HasPrecision(18, 2);
        b.Property(e => e.ResponsavelId).HasMaxLength(36);
        b.Property(e => e.Cor).HasMaxLength(10);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasOne(e => e.AtividadePai).WithMany(e => e.SubAtividades).HasForeignKey(e => e.AtividadePaiId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Recursos).WithOne(r => r.Atividade).HasForeignKey(r => r.AtividadeId).OnDelete(DeleteBehavior.Cascade);
    }
}
