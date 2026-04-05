using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Constriva.Domain.Entities.Lens;

namespace Constriva.Infrastructure.Persistence.Configurations;

public class DocumentoLensConfiguration : IEntityTypeConfiguration<DocumentoLens>
{
    public void Configure(EntityTypeBuilder<DocumentoLens> b)
    {
        b.ToTable("DocumentosLens");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.Property(e => e.NomeArquivo).HasMaxLength(500).IsRequired();
        b.Property(e => e.CaminhoArquivo).HasMaxLength(1000).IsRequired();
        b.Property(e => e.ExtensaoArquivo).HasMaxLength(20).IsRequired();
        b.Property(e => e.Warnings).HasColumnType("jsonb");
        b.Property(e => e.MensagemErro).HasMaxLength(2000);
        b.Property(e => e.CodigoErro).HasMaxLength(100);
        b.Property(e => e.TentativaNumero).HasDefaultValue(1);
        b.Property(e => e.Observacoes).HasMaxLength(500);
        b.Property(e => e.MotivoRejeicaoOuCancelamento).HasMaxLength(1000);
        b.Property(e => e.NumeroDocumentoExtraido).HasMaxLength(100);
        b.Property(e => e.DataEmissaoExtraida).HasMaxLength(50);
        b.Property(e => e.ValorTotalExtraido).HasPrecision(18, 2);
        b.Property(e => e.CnpjFornecedorExtraido).HasMaxLength(18);
        b.Property(e => e.NomeFornecedorExtraido).HasMaxLength(300);
        b.Property(e => e.MetodoExtracao).HasConversion<string>().HasMaxLength(20).IsRequired();

        b.HasIndex(e => e.EmpresaId);
        b.HasIndex(e => e.ObraId);
        b.HasIndex(e => e.Status);
        b.HasIndex(e => e.CreatedAt);
        b.HasIndex(e => e.CnpjFornecedorExtraido);

        b.HasMany(e => e.Itens)
            .WithOne(i => i.DocumentoLens)
            .HasForeignKey(i => i.DocumentoLensId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
