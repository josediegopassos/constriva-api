using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class InspecaoConfiguration : IEntityTypeConfiguration<Inspecao>
{
    public void Configure(EntityTypeBuilder<Inspecao> b)
    {
        b.ToTable("Inspecoes");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.EmpresaId, e.Numero }).IsUnique();
        b.Property(e => e.Titulo).HasMaxLength(300).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(2000);
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.Localicacao).HasMaxLength(200);
        b.Property(e => e.ResponsavelInsId).HasMaxLength(36);
        b.Property(e => e.Resultado).HasMaxLength(50);
        b.Property(e => e.Observacoes).HasMaxLength(2000);
        b.HasMany(e => e.Itens).WithOne(i => i.Inspecao).HasForeignKey(i => i.InspecaoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Fotos).WithOne(f => f.Inspecao).HasForeignKey(f => f.InspecaoId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.NaoConformidades).WithOne().HasForeignKey(nc => nc.InspecaoId).OnDelete(DeleteBehavior.Restrict);
    }
}
