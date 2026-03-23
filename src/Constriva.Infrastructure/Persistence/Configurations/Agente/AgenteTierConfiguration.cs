using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteTierConfiguration : IEntityTypeConfiguration<AgenteTier>
{
    public void Configure(EntityTypeBuilder<AgenteTier> b)
    {
        b.ToTable("AgenteTiers");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(100).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
    }
}
