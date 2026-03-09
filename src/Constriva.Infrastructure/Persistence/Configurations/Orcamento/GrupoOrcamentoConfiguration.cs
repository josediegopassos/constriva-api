using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class GrupoOrcamentoConfiguration : IEntityTypeConfiguration<GrupoOrcamento>
{
    public void Configure(EntityTypeBuilder<GrupoOrcamento> b)
    {
        b.ToTable("GruposOrcamento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Nome).HasMaxLength(300).IsRequired();
        b.Property(e => e.ValorTotal).HasPrecision(18, 2);
        b.Property(e => e.PercentualTotal).HasPrecision(5, 2);
        b.HasOne(e => e.Orcamento).WithMany(o => o.Grupos).HasForeignKey(e => e.OrcamentoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.GrupoPai).WithMany(e => e.SubGrupos).HasForeignKey(e => e.GrupoPaiId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Itens).WithOne(i => i.Grupo).HasForeignKey(i => i.GrupoId).OnDelete(DeleteBehavior.Cascade);
    }
}
