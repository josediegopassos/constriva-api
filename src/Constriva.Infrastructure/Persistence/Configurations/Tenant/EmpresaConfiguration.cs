using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> b)
    {
        b.ToTable("Empresas");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.RazaoSocial).HasMaxLength(200).IsRequired();
        b.Property(e => e.NomeFantasia).HasMaxLength(200).IsRequired();
        b.Property(e => e.Cnpj).HasMaxLength(14).IsRequired();
        b.HasIndex(e => e.Cnpj).IsUnique();
        b.Property(e => e.InscricaoEstadual).HasMaxLength(30);
        b.Property(e => e.InscricaoMunicipal).HasMaxLength(30);
        b.Property(e => e.Email).HasMaxLength(200).IsRequired();
        b.Property(e => e.Telefone).HasMaxLength(20);
        b.Property(e => e.Site).HasMaxLength(300);
        b.Property(e => e.LogoUrl).HasMaxLength(1000);
        b.Property(e => e.Plano).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Logradouro).HasMaxLength(300);
        b.Property(e => e.Numero).HasMaxLength(20);
        b.Property(e => e.Complemento).HasMaxLength(100);
        b.Property(e => e.Bairro).HasMaxLength(100);
        b.Property(e => e.Cidade).HasMaxLength(100);
        b.Property(e => e.Estado).HasMaxLength(2);
        b.Property(e => e.Cep).HasMaxLength(10);
        b.Property(e => e.ConfiguracoesJson).HasColumnType("jsonb");
        b.HasMany(e => e.Usuarios).WithOne(u => u.Empresa).HasForeignKey(u => u.EmpresaId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.HistoricoPlanos).WithOne(h => h.Empresa).HasForeignKey(h => h.EmpresaId).OnDelete(DeleteBehavior.Cascade);
    }
}
