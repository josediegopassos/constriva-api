using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Contratos;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class MedicaoContratualConfiguration : IEntityTypeConfiguration<MedicaoContratual>
{
    public void Configure(EntityTypeBuilder<MedicaoContratual> b)
    {
        b.ToTable("MedicoesContratuais");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.ValorMedicao).HasPrecision(18, 2);
        b.Property(e => e.ValorRetencao).HasPrecision(18, 2);
        b.Ignore(e => e.ValorLiquido);
        b.Property(e => e.PercentualMedicao).HasPrecision(5, 2);
        b.Property(e => e.PercentualAcumulado).HasPrecision(5, 2);
        b.Property(e => e.MotivoRejeicao).HasMaxLength(500);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.Property(e => e.ArquivoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Contrato).WithMany(c => c.Medicoes).HasForeignKey(e => e.ContratoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Itens).WithOne(i => i.Medicao).HasForeignKey(i => i.MedicaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
