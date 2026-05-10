using MassTransit;
using Microsoft.Extensions.Logging;
using Constriva.Messaging.Contracts.WhatsApp.Events;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class CotacaoEnviadaFornecedorConsumer : IConsumer<CotacaoEnviadaFornecedorEvent>
{
    private readonly ISignalRNotificationService _notificacao;
    private readonly ILogger<CotacaoEnviadaFornecedorConsumer> _logger;

    public CotacaoEnviadaFornecedorConsumer(
        ISignalRNotificationService notificacao, ILogger<CotacaoEnviadaFornecedorConsumer> logger)
    { _notificacao = notificacao; _logger = logger; }

    public async Task Consume(ConsumeContext<CotacaoEnviadaFornecedorEvent> context)
    {
        var msg = context.Message;
        await _notificacao.NotificarWhatsAppAtualizacaoAsync(
            msg.CotacaoId, msg.EmpresaId,
            new { tipo = "cotacao_enviada_fornecedor", msg.FornecedorCotacaoId,
                  msg.FornecedorId, msg.WaMessageId, msg.EnviadaEm, msg.Lembrete },
            context.CancellationToken);

        _logger.LogInformation("SignalR: cotação enviada para fornecedor {F} [Lembrete: {L}]",
            msg.FornecedorId, msg.Lembrete);
    }
}
