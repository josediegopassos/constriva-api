using Constriva.Domain.Entities.WhatsApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Constriva.Infrastructure.Persistence.Configurations.WhatsApp;

public class RespostaFornecedorWhatsAppConfiguration : IEntityTypeConfiguration<RespostaFornecedorWhatsApp>
{
    public void Configure(EntityTypeBuilder<RespostaFornecedorWhatsApp> b)
    {
        b.ToTable("RespostasFornecedorWhatsApp");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();

        b.Property(e => e.CotacaoWhatsAppId).IsRequired();
        b.Property(e => e.FornecedorCotacaoId).IsRequired();
        b.Property(e => e.FornecedorId).IsRequired();
        b.Property(e => e.WaMessageId).HasMaxLength(100).IsRequired();
        b.Property(e => e.TelefoneOrigem).HasMaxLength(20).IsRequired();
        b.Property(e => e.RecebidaEm).IsRequired();
        b.Property(e => e.TipoConteudo).HasConversion<int>();
        // TextoMensagem: texto longo — sem HasMaxLength
        b.Property(e => e.WaMediaId).HasMaxLength(100);
        b.Property(e => e.MediaUrl).HasMaxLength(500);
        b.Property(e => e.MediaMimeType).HasMaxLength(100);
        b.Property(e => e.MediaNomeArquivo).HasMaxLength(255);
        b.Property(e => e.MediaPathArmazenado).HasMaxLength(500);
        // PayloadWebhookOriginal: texto longo — sem HasMaxLength
        b.Property(e => e.ProcessadoPelaIa).HasDefaultValue(false);
        b.Property(e => e.ExtraidaComSucesso).HasDefaultValue(false);
        b.Property(e => e.DescricaoFalha).HasMaxLength(1000);
        b.Property(e => e.TentativasProcessamento).HasDefaultValue(0);

        // Índices
        b.HasIndex(e => e.EmpresaId);
        b.HasIndex(e => e.CotacaoWhatsAppId);
        b.HasIndex(e => e.FornecedorId);
        b.HasIndex(e => e.FornecedorCotacaoId);
        b.HasIndex(e => e.WaMessageId).IsUnique();
        b.HasIndex(e => new { e.TelefoneOrigem, e.RecebidaEm });
        b.HasIndex(e => new { e.EmpresaId, e.ProcessadoPelaIa });
    }
}
