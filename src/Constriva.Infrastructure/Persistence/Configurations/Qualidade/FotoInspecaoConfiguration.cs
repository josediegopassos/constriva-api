using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Qualidade;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class FotoInspecaoConfiguration : IEntityTypeConfiguration<FotoInspecao>
{
    public void Configure(EntityTypeBuilder<FotoInspecao> b)
    {
        b.ToTable("FotosInspecao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Url).HasMaxLength(1000).IsRequired();
        b.Property(e => e.Legenda).HasMaxLength(300);
        b.HasOne(e => e.Inspecao).WithMany(i => i.Fotos).HasForeignKey(e => e.InspecaoId).OnDelete(DeleteBehavior.Cascade);
    }
}
