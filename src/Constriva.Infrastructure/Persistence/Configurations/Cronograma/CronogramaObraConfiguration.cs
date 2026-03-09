using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class CronogramaObraConfiguration : IEntityTypeConfiguration<CronogramaObra>
{
    public void Configure(EntityTypeBuilder<CronogramaObra> b)
    {
        b.ToTable("CronogramasObra");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(1000);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Atividades).WithOne(a => a.Cronograma).HasForeignKey(a => a.CronogramaId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.CurvaS).WithOne(c => c.Cronograma).HasForeignKey(c => c.CronogramaId).OnDelete(DeleteBehavior.Cascade);
    }
}
