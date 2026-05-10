using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.API.Consumers.WhatsApp;

public class WhatsAppAprovacaoProcessadaConsumer(
    IHubContext<CotacaoWhatsAppHub> hub,
    ILogger<WhatsAppAprovacaoProcessadaConsumer> logger)
    : WhatsAppConsumerBase(hub), IConsumer<WhatsAppAprovacaoProcessadaEvent>
{
    public async Task Consume(ConsumeContext<WhatsAppAprovacaoProcessadaEvent> context)
    {
        var msg = context.Message;
        await NotificarGrupoAsync(msg.CotacaoId, msg.EmpresaId, new
        {
            tipo = "cotacao_aprovada",
            msg.CotacaoId, msg.PropostaCotacaoId,
            msg.NomeFornecedorVencedor, msg.ValorTotalAprovado,
            msg.NumeroPedidoCompra, msg.AprovadaEm
        }, context.CancellationToken);

        logger.LogInformation("SignalR: aprovação processada. Fornecedor: {F} | PO: {PO}",
            msg.NomeFornecedorVencedor, msg.NumeroPedidoCompra);
    }
}
