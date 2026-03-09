using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FaseObraConfiguration : IEntityTypeConfiguration<FaseObra>
{
    public void Configure(EntityTypeBuilder<FaseObra> b)
    {
        b.ToTable("FasesObra");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.PercentualConcluido).HasPrecision(5, 2);
        b.Property(e => e.ValorPrevisto).HasPrecision(18, 2);
        b.Property(e => e.ValorRealizado).HasPrecision(18, 2);
        b.Property(e => e.Cor).HasMaxLength(10);
        b.HasOne(e => e.FasePai).WithMany(e => e.SubFases).HasForeignKey(e => e.FasePaiId).OnDelete(DeleteBehavior.Restrict);
    }
}
