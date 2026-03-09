using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras.Diario;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RDOEquipeConfiguration : IEntityTypeConfiguration<RDOEquipe>
{
    public void Configure(EntityTypeBuilder<RDOEquipe> b)
    {
        b.ToTable("RDOEquipes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Funcao).HasMaxLength(100).IsRequired();
        b.Property(e => e.HorasTrabalhadas).HasPrecision(5, 2);
        b.Property(e => e.Empresa).HasMaxLength(200);
        b.HasOne(e => e.RDO).WithMany(r => r.Equipes).HasForeignKey(e => e.RDOId).OnDelete(DeleteBehavior.Cascade);
    }
}
