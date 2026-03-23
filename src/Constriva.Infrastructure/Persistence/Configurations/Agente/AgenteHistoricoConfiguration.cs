using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteHistoricoConfiguration : IEntityTypeConfiguration<AgenteHistorico>
{
    public void Configure(EntityTypeBuilder<AgenteHistorico> b)
    {
        b.ToTable("AgenteHistorico");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Role).HasConversion<int>();
        b.Property(e => e.Conteudo).IsRequired();
    }
}
