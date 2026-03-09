using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class GrupoMaterialConfiguration : IEntityTypeConfiguration<GrupoMaterial>
{
    public void Configure(EntityTypeBuilder<GrupoMaterial> b)
    {
        b.ToTable("GruposMaterial");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.HasOne(e => e.GrupoPai).WithMany(e => e.SubGrupos).HasForeignKey(e => e.GrupoPaiId).OnDelete(DeleteBehavior.Restrict);
    }
}
