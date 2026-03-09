using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.GED;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class PastaDocumentoConfiguration : IEntityTypeConfiguration<PastaDocumento>
{
    public void Configure(EntityTypeBuilder<PastaDocumento> b)
    {
        b.ToTable("PastasDocumento");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Nome).HasMaxLength(200).IsRequired();
        b.Property(e => e.Descricao).HasMaxLength(500);
        b.Property(e => e.Cor).HasMaxLength(10);
        b.Property(e => e.CaminhoCompleto).HasMaxLength(2000);
        b.HasOne(e => e.PastaPai).WithMany(e => e.SubPastas).HasForeignKey(e => e.PastaPaiId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Documentos).WithOne(d => d.Pasta).HasForeignKey(d => d.PastaId).OnDelete(DeleteBehavior.Restrict);
    }
}
