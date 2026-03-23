using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Common;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> b)
    {
        b.ToTable("Enderecos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Logradouro).HasMaxLength(300);
        b.Property(e => e.Numero).HasMaxLength(20);
        b.Property(e => e.Complemento).HasMaxLength(100);
        b.Property(e => e.Bairro).HasMaxLength(100);
        b.Property(e => e.Cidade).HasMaxLength(100);
        b.Property(e => e.Estado).HasMaxLength(2);
        b.Property(e => e.Cep).HasMaxLength(10);
        b.Property(e => e.Latitude).HasPrecision(10, 7);
        b.Property(e => e.Longitude).HasPrecision(10, 7);
    }
}
