using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Cronograma;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class VinculoAtividadeConfiguration : IEntityTypeConfiguration<VinculoAtividade>
{
    public void Configure(EntityTypeBuilder<VinculoAtividade> b)
    {
        b.ToTable("VinculosAtividade");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Tipo).HasConversion<int>();
        b.HasOne(e => e.AtividadeOrigem).WithMany(a => a.Sucessoras).HasForeignKey(e => e.AtividadeOrigemId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.AtividadeDestino).WithMany(a => a.Predecessoras).HasForeignKey(e => e.AtividadeDestinoId).OnDelete(DeleteBehavior.Restrict);
    }
}
