using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.RH;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> b)
    {
        b.ToTable("Departamentos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.HasOne(e => e.Gestor).WithMany().HasForeignKey(e => e.GestorId).OnDelete(DeleteBehavior.SetNull);
        b.HasOne(e => e.DepartamentoPai).WithMany().HasForeignKey(e => e.DepartamentoPaiId).OnDelete(DeleteBehavior.Restrict);
    }
}
