using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteSessaoConfiguration : IEntityTypeConfiguration<AgenteSessao>
{
    public void Configure(EntityTypeBuilder<AgenteSessao> b)
    {
        b.ToTable("AgenteSessoes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasMany(e => e.Mensagens).WithOne(m => m.Sessao).HasForeignKey(m => m.SessaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
