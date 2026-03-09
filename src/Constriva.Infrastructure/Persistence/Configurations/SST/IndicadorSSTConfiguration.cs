using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class IndicadorSSTConfiguration : IEntityTypeConfiguration<IndicadorSST>
{
    public void Configure(EntityTypeBuilder<IndicadorSST> b)
    {
        b.ToTable("IndicadoresSST");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.ObraId, e.Competencia }).IsUnique();
        b.Property(e => e.Competencia).HasMaxLength(7).IsRequired();
        b.Ignore(e => e.TaxaFrequencia);
        b.Ignore(e => e.TaxaGravidade);
    }
}
