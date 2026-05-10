using Constriva.Domain.Entities.WhatsApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Constriva.Infrastructure.Persistence.Configurations.WhatsApp;

public class MensagemWhatsAppConfiguration : IEntityTypeConfiguration<MensagemWhatsApp>
{
    public void Configure(EntityTypeBuilder<MensagemWhatsApp> b)
    {
        b.ToTable("MensagensWhatsApp");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();

        b.Property(e => e.CotacaoWhatsAppId).IsRequired();
        b.Property(e => e.FornecedorCotacaoId).IsRequired();
        b.Property(e => e.FornecedorId).IsRequired();
        b.Property(e => e.TelefoneDestino).HasMaxLength(20).IsRequired();
        b.Property(e => e.NomeFornecedor).HasMaxLength(200).IsRequired();
        b.Property(e => e.TipoMensagem).HasConversion<int>();
        b.Property(e => e.Status).HasConversion<int>();
        b.Property(e => e.WaMessageId).HasMaxLength(100);
        b.Property(e => e.NumeroTentativa).HasDefaultValue(1);
        b.Property(e => e.ErroEnvio).HasMaxLength(500);
        // PayloadEnviado: texto longo — sem HasMaxLength

        // Índices
        b.HasIndex(e => e.EmpresaId);
        b.HasIndex(e => e.CotacaoWhatsAppId);
        b.HasIndex(e => e.FornecedorId);
        b.HasIndex(e => e.WaMessageId).IsUnique().HasFilter("\"WaMessageId\" IS NOT NULL");
        b.HasIndex(e => new { e.EmpresaId, e.Status });
        b.HasIndex(e => new { e.FornecedorCotacaoId, e.TipoMensagem });
    }
}
