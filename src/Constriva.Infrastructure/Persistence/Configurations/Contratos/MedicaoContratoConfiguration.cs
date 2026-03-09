using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class MedicaoContratoConfiguration : IEntityTypeConfiguration<MedicaoContrato>
{
    public void Configure(EntityTypeBuilder<MedicaoContrato> b)
    {
        b.ToTable("MedicoesContrato");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.ValorMedicao).HasPrecision(18, 2);
        b.Property(e => e.ValorAcumulado).HasPrecision(18, 2);
        b.Property(e => e.PercentualMedicao).HasPrecision(5, 2);
        b.Property(e => e.PercentualAcumulado).HasPrecision(5, 2);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Itens).WithOne(i => i.Medicao).HasForeignKey(i => i.MedicaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
