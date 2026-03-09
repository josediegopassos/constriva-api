using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.SST;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ParticipateDDSConfiguration : IEntityTypeConfiguration<ParticipateDDS>
{
    public void Configure(EntityTypeBuilder<ParticipateDDS> b)
    {
        b.ToTable("ParticipantesDDS");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Funcao).HasMaxLength(100);
        b.Property(e => e.Empresa).HasMaxLength(200);
        b.Property(e => e.AssinaturaUrl).HasMaxLength(1000);
        b.HasOne(e => e.DDS).WithMany(d => d.Participantes).HasForeignKey(e => e.DDSId).OnDelete(DeleteBehavior.Cascade);
    }
}
