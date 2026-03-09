using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemCotacaoConfiguration : IEntityTypeConfiguration<ItemCotacao>
{
    public void Configure(EntityTypeBuilder<ItemCotacao> b)
    {
        b.ToTable("ItensCotacao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.UnidadeMedida).HasMaxLength(20).IsRequired();
        b.Property(e => e.Quantidade).HasPrecision(18, 4);
        b.Property(e => e.Especificacao).HasMaxLength(2000);
        b.Property(e => e.PrecoReferencia).HasPrecision(18, 4);
        b.HasOne(e => e.Cotacao).WithMany(c => c.Itens).HasForeignKey(e => e.CotacaoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
