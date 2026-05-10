using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.API.Consumers.WhatsApp;

public class WhatsAppEnvioFalhouConsumer(
    IHubContext<CotacaoWhatsAppHub> hub,
    ILogger<WhatsAppEnvioFalhouConsumer> logger)
    : WhatsAppConsumerBase(hub), IConsumer<WhatsAppEnvioFalhouEvent>
{
    public async Task Consume(ConsumeContext<WhatsAppEnvioFalhouEvent> context)
    {
        var msg = context.Message;
        await NotificarGrupoAsync(msg.CotacaoId, msg.EmpresaId, new
        {
            tipo = "envio_falhou",
            msg.FornecedorCotacaoId, msg.FornecedorId,
            msg.NomeFornecedor, msg.Erro, msg.FalhouEm
        }, context.CancellationToken);

        logger.LogWarning("SignalR: envio falhou para {F}: {Erro}", msg.NomeFornecedor, msg.Erro);
    }
}
