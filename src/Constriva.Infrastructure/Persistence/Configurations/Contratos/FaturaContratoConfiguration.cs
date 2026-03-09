using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Contratos;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FaturaContratoConfiguration : IEntityTypeConfiguration<FaturaContrato>
{
    public void Configure(EntityTypeBuilder<FaturaContrato> b)
    {
        b.ToTable("FaturasContrato");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(50).IsRequired();
        b.Property(e => e.NumeroNF).HasMaxLength(50);
        b.Property(e => e.ValorFatura).HasPrecision(18, 2);
        b.Property(e => e.ValorPago).HasPrecision(18, 2);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasOne(e => e.Contrato).WithMany(c => c.Faturas).HasForeignKey(e => e.ContratoId).OnDelete(DeleteBehavior.Cascade);
    }
}
