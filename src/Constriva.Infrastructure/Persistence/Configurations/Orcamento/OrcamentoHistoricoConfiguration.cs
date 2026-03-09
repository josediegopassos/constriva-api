using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class OrcamentoHistoricoConfiguration : IEntityTypeConfiguration<OrcamentoHistorico>
{
    public void Configure(EntityTypeBuilder<OrcamentoHistorico> b)
    {
        b.ToTable("OrcamentoHistoricos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.ValorAnterior).HasPrecision(18, 2);
        b.Property(e => e.ValorNovo).HasPrecision(18, 2);
        b.HasOne(e => e.Orcamento).WithMany(o => o.Historicos).HasForeignKey(e => e.OrcamentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
