using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class CurvaSPontoConfiguration : IEntityTypeConfiguration<CurvaSPonto>
{
    public void Configure(EntityTypeBuilder<CurvaSPonto> b)
    {
        b.ToTable("CurvaSPontos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.PercentualPrevisto).HasPrecision(5, 2);
        b.Property(e => e.PercentualRealizado).HasPrecision(5, 2);
        b.Property(e => e.ValorPrevisto).HasPrecision(18, 2);
        b.Property(e => e.ValorRealizado).HasPrecision(18, 2);
        b.HasOne(e => e.Cronograma).WithMany(c => c.CurvaS).HasForeignKey(e => e.CronogramaId).OnDelete(DeleteBehavior.Cascade);
    }
}
