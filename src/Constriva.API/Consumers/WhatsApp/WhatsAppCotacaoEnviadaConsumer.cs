using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.API.Consumers.WhatsApp;

public class WhatsAppCotacaoEnviadaConsumer(
    IHubContext<CotacaoWhatsAppHub> hub,
    ILogger<WhatsAppCotacaoEnviadaConsumer> logger)
    : WhatsAppConsumerBase(hub), IConsumer<CotacaoEnviadaFornecedorEvent>
{
    public async Task Consume(ConsumeContext<CotacaoEnviadaFornecedorEvent> context)
    {
        var msg = context.Message;
        await NotificarGrupoAsync(msg.CotacaoId, msg.EmpresaId, new
        {
            tipo = "cotacao_enviada_fornecedor",
            msg.FornecedorCotacaoId, msg.FornecedorId,
            msg.WaMessageId, msg.EnviadaEm, msg.Lembrete
        }, context.CancellationToken);

        logger.LogInformation("SignalR: cotação enviada para fornecedor {F} [Lembrete: {L}]",
            msg.FornecedorId, msg.Lembrete);
    }
}
