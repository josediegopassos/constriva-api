using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteCotaAvulsaConfiguration : IEntityTypeConfiguration<AgenteCotaAvulsa>
{
    public void Configure(EntityTypeBuilder<AgenteCotaAvulsa> b)
    {
        b.ToTable("AgenteCotasAvulsas");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Motivo).HasMaxLength(500);
        b.Ignore(e => e.TokensRestantes);
    }
}
