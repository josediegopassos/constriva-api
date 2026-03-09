using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ObraHistoricoConfiguration : IEntityTypeConfiguration<ObraHistorico>
{
    public void Configure(EntityTypeBuilder<ObraHistorico> b)
    {
        b.ToTable("ObraHistoricos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Acao).HasMaxLength(100).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000).IsRequired();
        b.Property(e => e.ValorAnterior).HasMaxLength(2000);
        b.Property(e => e.ValorNovo).HasMaxLength(2000);
        b.HasOne(e => e.Obra).WithMany(o => o.Historicos).HasForeignKey(e => e.ObraId).OnDelete(DeleteBehavior.Cascade);
    }
}
