using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemMedicaoConfiguration : IEntityTypeConfiguration<ItemMedicao>
{
    public void Configure(EntityTypeBuilder<ItemMedicao> b)
    {
        b.ToTable("ItensMedicao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.QuantidadeContratada).HasPrecision(18, 4);
        b.Property(e => e.QuantidadeAnterior).HasPrecision(18, 4);
        b.Property(e => e.QuantidadeAtual).HasPrecision(18, 4);
        b.Ignore(e => e.QuantidadeAcumulada);
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Ignore(e => e.ValorAtual);
        b.HasOne(e => e.Medicao).WithMany(m => m.Itens).HasForeignKey(e => e.MedicaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
