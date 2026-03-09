using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class UsuarioPermissaoConfiguration : IEntityTypeConfiguration<UsuarioPermissao>
{
    public void Configure(EntityTypeBuilder<UsuarioPermissao> b)
    {
        b.ToTable("UsuarioPermissoes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Modulo).HasMaxLength(50).IsRequired();
        b.HasIndex(e => new { e.UsuarioId, e.Modulo, e.EmpresaId }).IsUnique();
    }
}
