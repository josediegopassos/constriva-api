using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Agente;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AgenteEmpresaConfigConfiguration : IEntityTypeConfiguration<AgenteEmpresaConfig>
{
    public void Configure(EntityTypeBuilder<AgenteEmpresaConfig> b)
    {
        b.ToTable("AgenteEmpresaConfigs");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.HasOne(e => e.Tier).WithMany().HasForeignKey(e => e.AgenteTierId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(e => e.EmpresaId).IsUnique();
    }
}
