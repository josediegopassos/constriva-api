using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("Usuarios");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Email).HasMaxLength(200).IsRequired();
        b.HasIndex(e => new { e.Email, e.EmpresaId }).IsUnique();
        b.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
        b.Property(e => e.Telefone).HasMaxLength(20);
        b.Property(e => e.Cargo).HasMaxLength(100);
        b.Property(e => e.AvatarUrl).HasMaxLength(1000);
        b.Property(e => e.Perfil).HasConversion<int>();
        b.Property(e => e.RefreshToken).HasMaxLength(500);
        b.Property(e => e.TokenConfirmacaoEmail).HasMaxLength(200);
        b.Property(e => e.TokenRedefinicaoSenha).HasMaxLength(200);
        b.Property(e => e.TimeZone).HasMaxLength(50);
        b.Property(e => e.Idioma).HasMaxLength(10);
        b.HasMany(e => e.Permissoes).WithOne(p => p.Usuario).HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
