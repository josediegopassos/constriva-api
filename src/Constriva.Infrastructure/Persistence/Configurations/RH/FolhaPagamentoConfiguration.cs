using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FolhaPagamentoConfiguration : IEntityTypeConfiguration<FolhaPagamento>
{
    public void Configure(EntityTypeBuilder<FolhaPagamento> b)
    {
        b.ToTable("FolhasPagamento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Competencia).HasMaxLength(7).IsRequired();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.ValorTotalBruto).HasPrecision(18, 2);
        b.Property(e => e.ValorTotalDescontos).HasPrecision(18, 2);
        b.Property(e => e.ValorTotalLiquido).HasPrecision(18, 2);
        b.HasMany(e => e.Funcionarios).WithOne(f => f.Folha).HasForeignKey(f => f.FolhaId).OnDelete(DeleteBehavior.Cascade);
    }
}
