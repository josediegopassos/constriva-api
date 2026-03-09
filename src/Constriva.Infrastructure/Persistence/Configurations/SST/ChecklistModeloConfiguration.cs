using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ChecklistModeloConfiguration : IEntityTypeConfiguration<ChecklistModelo>
{
    public void Configure(EntityTypeBuilder<ChecklistModelo> b)
    {
        b.ToTable("ChecklistModelos");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(1000);
        b.Property(e => e.Categoria).HasMaxLength(100);
        b.Property(e => e.EtapaConstrucao).HasMaxLength(100);
        b.HasMany(e => e.Itens).WithOne(i => i.Modelo).HasForeignKey(i => i.ModeloId).OnDelete(DeleteBehavior.Cascade);
    }
}
