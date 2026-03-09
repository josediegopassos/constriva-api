using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Compras;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemPropostaCotacaoConfiguration : IEntityTypeConfiguration<ItemPropostaCotacao>
{
    public void Configure(EntityTypeBuilder<ItemPropostaCotacao> b)
    {
        b.ToTable("ItensPropostaCotacao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Property(e => e.Quantidade).HasPrecision(18, 4);
        b.Ignore(e => e.ValorTotal);
        b.Property(e => e.Marca).HasMaxLength(100);
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.HasOne(e => e.Proposta).WithMany(p => p.Itens).HasForeignKey(e => e.PropostaId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.ItemCotacao).WithMany().HasForeignKey(e => e.ItemCotacaoId).OnDelete(DeleteBehavior.Restrict);
    }
}
