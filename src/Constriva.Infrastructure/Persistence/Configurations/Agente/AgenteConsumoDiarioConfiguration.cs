using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteConsumoDiarioConfiguration : IEntityTypeConfiguration<AgenteConsumoDiario>
{
    public void Configure(EntityTypeBuilder<AgenteConsumoDiario> b)
    {
        b.ToTable("AgenteConsumoDiario");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.EmpresaId, e.Data }).IsUnique();
    }
}
