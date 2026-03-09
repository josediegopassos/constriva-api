using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class UsuarioObraConfiguration : IEntityTypeConfiguration<UsuarioObra>
{
    public void Configure(EntityTypeBuilder<UsuarioObra> b)
    {
        b.ToTable("UsuarioObras");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Funcao).HasMaxLength(100).IsRequired();
        b.HasIndex(e => new { e.UsuarioId, e.ObraId }).IsUnique();
        b.HasOne(e => e.Usuario).WithMany(u => u.ObrasVinculadas).HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
