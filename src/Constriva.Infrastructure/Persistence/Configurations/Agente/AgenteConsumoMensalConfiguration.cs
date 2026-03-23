using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteConsumoMensalConfiguration : IEntityTypeConfiguration<AgenteConsumoMensal>
{
    public void Configure(EntityTypeBuilder<AgenteConsumoMensal> b)
    {
        b.ToTable("AgenteConsumoMensal");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasIndex(e => new { e.EmpresaId, e.Ano, e.Mes }).IsUnique();
    }
}
