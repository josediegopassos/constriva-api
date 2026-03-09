using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Estoque;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemRequisicaoConfiguration : IEntityTypeConfiguration<ItemRequisicao>
{
    public void Configure(EntityTypeBuilder<ItemRequisicao> b)
    {
        b.ToTable("ItensRequisicao");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();
        b.Property(e => e.QuantidadeSolicitada).HasPrecision(18, 4);
        b.Property(e => e.QuantidadeAtendida).HasPrecision(18, 4);
        b.Property(e => e.PrecoReferencia).HasPrecision(18, 4);
        b.Property(e => e.Observacao).HasMaxLength(500);
        b.HasOne(e => e.Requisicao).WithMany(r => r.Itens).HasForeignKey(e => e.RequisicaoId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(e => e.Material).WithMany().HasForeignKey(e => e.MaterialId).OnDelete(DeleteBehavior.Restrict);
    }
}
