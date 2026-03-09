using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FluxoAprovacaoDocConfiguration : IEntityTypeConfiguration<FluxoAprovacaoDoc>
{
    public void Configure(EntityTypeBuilder<FluxoAprovacaoDoc> b)
    {
        b.ToTable("FluxosAprovacaoDoc");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Status).HasMaxLength(20).IsRequired();
        b.Property(e => e.Comentario).HasMaxLength(1000);
        b.HasOne(e => e.Documento).WithMany(d => d.FluxoAprovacao).HasForeignKey(e => e.DocumentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
