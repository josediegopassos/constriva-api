using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Obras.Diario;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class RDOConfiguration : IEntityTypeConfiguration<RDO>
{
    public void Configure(EntityTypeBuilder<RDO> b)
    {
        b.ToTable("RDOs");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Numero).HasMaxLength(20).IsRequired();
        b.HasIndex(e => new { e.ObraId, e.Numero }).IsUnique();
        b.Property(e => e.CondicaoTempo).HasMaxLength(100);
        b.Property(e => e.TempoManha).HasMaxLength(50);
        b.Property(e => e.TempoTarde).HasMaxLength(50);
        b.Property(e => e.TempoNoite).HasMaxLength(50);
        b.Property(e => e.TemperaturaMin).HasPrecision(5, 1);
        b.Property(e => e.TemperaturaMax).HasPrecision(5, 1);
        b.Property(e => e.AtividadesRealizadas).HasMaxLength(4000);
        b.Property(e => e.OcorrenciasObservacoes).HasMaxLength(2000);
        b.Property(e => e.PendenciasProblemas).HasMaxLength(2000);
        b.Property(e => e.Fotografias).HasColumnType("jsonb");
        b.Property(e => e.ResponsavelNome).HasMaxLength(200);
        b.Property(e => e.ResponsavelAssinaturaUrl).HasMaxLength(1000);
        b.HasMany(e => e.Equipes).WithOne(eq => eq.RDO).HasForeignKey(eq => eq.RDOId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Equipamentos).WithOne(eq => eq.RDO).HasForeignKey(eq => eq.RDOId).OnDelete(DeleteBehavior.Cascade);
        b.HasMany(e => e.Ocorrencias).WithOne(o => o.RDO).HasForeignKey(o => o.RDOId).OnDelete(DeleteBehavior.Cascade);
    }
}
