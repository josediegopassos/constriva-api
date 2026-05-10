using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constriva.Domain.Entities.WhatsApp;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Infrastructure.Integrations.WhatsApp;
using Constriva.Infrastructure.Persistence;
using Constriva.Infrastructure.Integrations.WhatsApp.Options;
using Constriva.Messaging.Contracts.WhatsApp.Commands;
using Constriva.Messaging.Contracts.WhatsApp.Events;
using Microsoft.Extensions.Options;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class EnviarLembreteCotacaoConsumer : IConsumer<EnviarLembreteCotacaoCommand>
{
    private readonly IWhatsAppGateway _gateway;
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publish;
    private readonly ILogger<EnviarLembreteCotacaoConsumer> _logger;
    private readonly WhatsAppOptions _options;

    public EnviarLembreteCotacaoConsumer(
        IWhatsAppGateway gateway, AppDbContext db,
        IPublishEndpoint publish, ILogger<EnviarLembreteCotacaoConsumer> logger,
        IOptions<WhatsAppOptions> options)
    {
        _gateway = gateway;
        _db = db;
        _publish = publish;
        _logger = logger;
        _options = options.Value;
    }

    public async Task Consume(ConsumeContext<EnviarLembreteCotacaoCommand> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        var jaRespondeu = await _db.RespostasFornecedorWhatsApp.AnyAsync(r =>
            r.FornecedorCotacaoId == msg.FornecedorCotacaoId &&
            r.EmpresaId == msg.EmpresaId && !r.IsDeleted, ct);
        if (jaRespondeu) { _logger.LogInformation("Fornecedor {F} já respondeu — lembrete cancelado", msg.NomeFornecedor); return; }
        if (DateTime.UtcNow > msg.DataLimiteResposta) { _logger.LogInformation("Prazo expirado para cotação {N}", msg.NumeroCotacao); return; }
        if (msg.NumeroTentativa > _options.MaxLembretes) { _logger.LogWarning("Limite de lembretes ({Max}) atingido para {F}", _options.MaxLembretes, msg.NomeFornecedor); return; }

        try
        {
            var waMessageId = await _gateway.EnviarLembreteCotacaoAsync(
                msg.TelefoneWhatsApp, msg.NomeFornecedor, msg.NumeroCotacao,
                msg.DataLimiteResposta, msg.NumeroTentativa, ct);

            var cotacaoWa = await _db.CotacoesWhatsApp.FirstAsync(c =>
                c.CotacaoId == msg.CotacaoId && c.EmpresaId == msg.EmpresaId, ct);

            var lembrete = new MensagemWhatsApp(
                msg.EmpresaId, cotacaoWa.Id, msg.FornecedorCotacaoId, msg.FornecedorId,
                msg.TelefoneWhatsApp, msg.NomeFornecedor,
                TipoMensagemWhatsAppEnum.LembreteCotacao,
                $"Lembrete #{msg.NumeroTentativa}", msg.NumeroTentativa);

            lembrete.MarcarComoEnviada(waMessageId);
            _db.MensagensWhatsApp.Add(lembrete);
            await _db.SaveChangesAsync(ct);

            await _publish.Publish(new CotacaoEnviadaFornecedorEvent
            {
                CotacaoId = msg.CotacaoId, FornecedorCotacaoId = msg.FornecedorCotacaoId,
                FornecedorId = msg.FornecedorId, EmpresaId = msg.EmpresaId,
                WaMessageId = waMessageId, EnviadaEm = DateTime.UtcNow, Lembrete = true
            }, ct);

            _logger.LogInformation("Lembrete #{T} enviado para {F}. WaMessageId: {W}",
                msg.NumeroTentativa, msg.NomeFornecedor, waMessageId);
        }
        catch (WhatsAppGatewayException ex)
        {
            _logger.LogError(ex, "Falha ao enviar lembrete para {F}", msg.NomeFornecedor);
            throw;
        }

        if (msg.NumeroTentativa < _options.MaxLembretes)
        {
            await context.SchedulePublish(DateTime.UtcNow.AddHours(24),
                new EnviarLembreteCotacaoCommand
                {
                    CotacaoId = msg.CotacaoId, FornecedorCotacaoId = msg.FornecedorCotacaoId,
                    FornecedorId = msg.FornecedorId, EmpresaId = msg.EmpresaId,
                    NumeroCotacao = msg.NumeroCotacao, TituloCotacao = msg.TituloCotacao,
                    NomeFornecedor = msg.NomeFornecedor, TelefoneWhatsApp = msg.TelefoneWhatsApp,
                    DataLimiteResposta = msg.DataLimiteResposta,
                    NumeroTentativa = msg.NumeroTentativa + 1,
                    EnvioOriginalEm = msg.EnvioOriginalEm
                });
        }
    }
}
