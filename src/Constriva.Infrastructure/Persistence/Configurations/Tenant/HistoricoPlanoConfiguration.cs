using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class HistoricoPlanoConfiguration : IEntityTypeConfiguration<HistoricoPlano>
{
    public void Configure(EntityTypeBuilder<HistoricoPlano> b)
    {
        b.ToTable("HistoricoPlanos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.PlanoAnterior).HasConversion<int>();
        b.Property(e => e.PlanoNovo).HasConversion<int>();
        b.Property(e => e.Motivo).HasMaxLength(500);
    }
}
