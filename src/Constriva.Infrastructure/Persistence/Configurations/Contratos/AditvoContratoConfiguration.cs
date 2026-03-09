using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Contratos;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AditvoContratoConfiguration : IEntityTypeConfiguration<AditvoContrato>
{
    public void Configure(EntityTypeBuilder<AditvoContrato> b)
    {
        b.ToTable("AditivosContrato");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(50).IsRequired();
        b.Property(e => e.Tipo).HasMaxLength(50).IsRequired();
        b.Property(e => e.Justificativa).HasMaxLength(2000).IsRequired();
        b.Property(e => e.ValorAditivo).HasPrecision(18, 2);
        b.Property(e => e.ArquivoUrl).HasMaxLength(1000);
        b.HasOne(e => e.Contrato).WithMany(c => c.Aditivos).HasForeignKey(e => e.ContratoId).OnDelete(DeleteBehavior.Cascade);
    }
}
