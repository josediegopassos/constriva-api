using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DDSConfiguration : IEntityTypeConfiguration<DDS>
{
    public void Configure(EntityTypeBuilder<DDS> b)
    {
        b.ToTable("DDS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Tema).HasMaxLength(300).IsRequired();
        b.Property(e => e.Conteudo).HasMaxLength(4000);
        b.Property(e => e.Ministrador).HasMaxLength(200);
        b.Property(e => e.DuracaoMinutos).HasPrecision(5, 0);
        b.Property(e => e.Local).HasMaxLength(200);
        b.Property(e => e.FotoUrl).HasMaxLength(1000);
        b.HasMany(e => e.Participantes).WithOne(p => p.DDS).HasForeignKey(p => p.DDSId).OnDelete(DeleteBehavior.Cascade);
    }
}
