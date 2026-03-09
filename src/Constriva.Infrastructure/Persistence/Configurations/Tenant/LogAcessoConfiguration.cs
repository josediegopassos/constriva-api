using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Tenant;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class LogAcessoConfiguration : IEntityTypeConfiguration<LogAcesso>
{
    public void Configure(EntityTypeBuilder<LogAcesso> b)
    {
        b.ToTable("LogAcessos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Acao).HasMaxLength(100).IsRequired();
        b.Property(e => e.IpAddress).HasMaxLength(50);
        b.Property(e => e.UserAgent).HasMaxLength(500);
        b.Property(e => e.Detalhes).HasMaxLength(2000);
        b.HasIndex(e => e.UsuarioId);
        b.HasOne(e => e.Usuario).WithMany(u => u.LogsAcesso).HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Cascade);
    }
}
