using Constriva.Domain.Entities.WhatsApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Constriva.Infrastructure.Persistence.Configurations.WhatsApp;

public class CotacaoWhatsAppConfiguration : IEntityTypeConfiguration<CotacaoWhatsApp>
{
    public void Configure(EntityTypeBuilder<CotacaoWhatsApp> b)
    {
        b.ToTable("CotacoesWhatsApp");
        b.HasKey(e => e.Id);
        b.Property(e => e.Id).IsRequired();
        b.HasIndex(e => e.Id).IsUnique();

        b.Property(e => e.CotacaoId).IsRequired();
        b.Property(e => e.TelefoneEmpresa).HasMaxLength(20).IsRequired();
        b.Property(e => e.NomeExibicaoEmpresa).HasMaxLength(100).IsRequired();
        b.Property(e => e.TotalFornecedoresConvidados).HasDefaultValue(0);
        b.Property(e => e.TotalRespostas).HasDefaultValue(0);
        b.Property(e => e.TotalPropostasExtraidas).HasDefaultValue(0);
        b.Property(e => e.MensagemPersonalizada).HasMaxLength(1000);

        // Relação 1:1 lógica com Cotacao (sem FK de banco — cross-domain)
        b.HasIndex(e => e.CotacaoId).IsUnique();

        // Índices
        b.HasIndex(e => e.EmpresaId);
        b.HasIndex(e => new { e.EmpresaId, e.DataLimiteResposta });

        // Relacionamentos 1:N
        b.HasMany(e => e.Mensagens)
            .WithOne(e => e.CotacaoWhatsApp)
            .HasForeignKey(e => e.CotacaoWhatsAppId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(e => e.Respostas)
            .WithOne(e => e.CotacaoWhatsApp)
            .HasForeignKey(e => e.CotacaoWhatsAppId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
