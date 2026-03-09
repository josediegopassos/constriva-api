using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ChecklistModeloItemConfiguration : IEntityTypeConfiguration<ChecklistModeloItem>
{
    public void Configure(EntityTypeBuilder<ChecklistModeloItem> b)
    {
        b.ToTable("ChecklistModeloItens");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.Criterio).HasMaxLength(1000);
        b.Property(e => e.TipoResposta).HasMaxLength(20).IsRequired();
        b.Property(e => e.NormaReferencia).HasMaxLength(100);
        b.HasOne(e => e.Modelo).WithMany(m => m.Itens).HasForeignKey(e => e.ModeloId).OnDelete(DeleteBehavior.Cascade);
    }
}
