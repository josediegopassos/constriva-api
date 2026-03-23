using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Clientes;

namespace Constriva.Infrastructure.Persistence.Configurations.Clientes;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> b)
    {
        b.ToTable("Clientes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();

        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Codigo }).IsUnique();

        b.Property(e => e.TipoPessoa).HasConversion<int>().IsRequired();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.NomeFantasia).HasMaxLength(200);
        b.Property(e => e.Documento).HasMaxLength(18);
        b.HasIndex(e => new { e.EmpresaId, e.Documento }).IsUnique()
            .HasFilter("\"Documento\" IS NOT NULL AND \"IsDeleted\" = false");

        b.Property(e => e.InscricaoEstadual).HasMaxLength(20);
        b.Property(e => e.InscricaoMunicipal).HasMaxLength(20);
        b.Property(e => e.Email).HasMaxLength(200);
        b.Property(e => e.Telefone).HasMaxLength(20);
        b.Property(e => e.Celular).HasMaxLength(20);
        b.Property(e => e.Site).HasMaxLength(300);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Observacoes).HasMaxLength(2000);

        b.HasOne(e => e.Endereco).WithMany().HasForeignKey(e => e.EnderecoId).OnDelete(DeleteBehavior.SetNull);

        b.HasMany(e => e.Obras)
            .WithOne(o => o.Cliente)
            .HasForeignKey(o => o.ClienteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
