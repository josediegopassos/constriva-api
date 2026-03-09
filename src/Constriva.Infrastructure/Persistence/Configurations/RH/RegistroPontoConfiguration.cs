using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RegistroPontoConfiguration : IEntityTypeConfiguration<RegistroPonto>
{
    public void Configure(EntityTypeBuilder<RegistroPonto> b)
    {
        b.ToTable("RegistrosPonto");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.Property(e => e.HorarioPrevisto).HasMaxLength(10);
        b.Property(e => e.HorasExtras).HasPrecision(5, 2);
        b.Property(e => e.Latitude).HasMaxLength(20);
        b.Property(e => e.Longitude).HasMaxLength(20);
        b.Property(e => e.Dispositivo).HasMaxLength(200);
        b.Property(e => e.Justificativa).HasMaxLength(500);
        b.HasOne(e => e.Funcionario).WithMany(f => f.RegistrosPonto).HasForeignKey(e => e.FuncionarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
