using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class AlmoxarifadoConfiguration : IEntityTypeConfiguration<Almoxarifado>
{
    public void Configure(EntityTypeBuilder<Almoxarifado> b)
    {
        b.ToTable("Almoxarifados");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.Property(e => e.Logradouro).HasMaxLength(300);
        b.Property(e => e.Cidade).HasMaxLength(100);
    }
}
