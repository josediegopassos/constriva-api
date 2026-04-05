using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Lens;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class ItemDocumentoLensConfiguration : IEntityTypeConfiguration<ItemDocumentoLens>
{
    public void Configure(EntityTypeBuilder<ItemDocumentoLens> b)
    {
        b.ToTable("ItensDocumentoLens");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.Property(e => e.Codigo).HasMaxLength(50);
        b.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
        b.Property(e => e.Ncm).HasMaxLength(20);
        b.Property(e => e.Cfop).HasMaxLength(10);
        b.Property(e => e.Unidade).HasMaxLength(20);
        b.Property(e => e.Quantidade).HasPrecision(18, 4);
        b.Property(e => e.PrecoUnitario).HasPrecision(18, 4);
        b.Property(e => e.PrecoTotal).HasPrecision(18, 2);
        b.Property(e => e.Desconto).HasPrecision(18, 2);
        b.Property(e => e.AliquotaIcms).HasPrecision(5, 2);
        b.Property(e => e.AliquotaIpi).HasPrecision(5, 2);
        b.Property(e => e.MotivoRejeicao).HasMaxLength(500);
        b.Property(e => e.DescricaoOriginalOcr).HasMaxLength(500);
        b.Property(e => e.QuantidadeOriginalOcr).HasPrecision(18, 4);
        b.Property(e => e.PrecoUnitarioOriginalOcr).HasPrecision(18, 4);
        b.Property(e => e.PrecoTotalOriginalOcr).HasPrecision(18, 2);

        b.HasIndex(e => e.DocumentoLensId);
        b.HasIndex(e => e.Status);
        b.HasIndex(e => e.ProdutoId);
    }
}
