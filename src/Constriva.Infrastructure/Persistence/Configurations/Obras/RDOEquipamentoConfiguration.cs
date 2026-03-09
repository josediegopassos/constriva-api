using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras.Diario;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RDOEquipamentoConfiguration : IEntityTypeConfiguration<RDOEquipamento>
{
    public void Configure(EntityTypeBuilder<RDOEquipamento> b)
    {
        b.ToTable("RDOEquipamentos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(300).IsRequired();
        b.Property(e => e.HorasUtilizadas).HasPrecision(5, 2);
        b.Property(e => e.Estado).HasMaxLength(50);
        b.HasOne(e => e.RDO).WithMany(r => r.Equipamentos).HasForeignKey(e => e.RDOId).OnDelete(DeleteBehavior.Cascade);
    }
}
