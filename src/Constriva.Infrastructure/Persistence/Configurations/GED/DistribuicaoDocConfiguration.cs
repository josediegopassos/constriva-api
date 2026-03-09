using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DistribuicaoDocConfiguration : IEntityTypeConfiguration<DistribuicaoDoc>
{
    public void Configure(EntityTypeBuilder<DistribuicaoDoc> b)
    {
        b.ToTable("DistribuicoesDoc");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.DestinatarioNome).HasMaxLength(200);
        b.Property(e => e.DestinatarioEmail).HasMaxLength(200);
        b.Property(e => e.Finalidade).HasMaxLength(50);
        b.HasOne(e => e.Documento).WithMany(d => d.Distribuicoes).HasForeignKey(e => e.DocumentoId).OnDelete(DeleteBehavior.Cascade);
    }
}
