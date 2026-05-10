using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.API.Consumers.WhatsApp;

public class WhatsAppPropostaExtraidaConsumer(
    IHubContext<CotacaoWhatsAppHub> hub,
    ILogger<WhatsAppPropostaExtraidaConsumer> logger)
    : WhatsAppConsumerBase(hub), IConsumer<PropostaExtraidaComSucessoEvent>
{
    public async Task Consume(ConsumeContext<PropostaExtraidaComSucessoEvent> context)
    {
        var msg = context.Message;
        await NotificarGrupoAsync(msg.CotacaoId, msg.EmpresaId, new
        {
            tipo = "proposta_extraida_com_sucesso",
            msg.FornecedorCotacaoId, msg.FornecedorId,
            msg.NivelConfianca, msg.CondicoesPagamento,
            msg.PrazoEntregaDias, totalItens = msg.ItensExtraidos.Count, msg.ExtraidaEm
        }, context.CancellationToken);

        logger.LogInformation("SignalR: proposta extraída. Confiança: {C} | Fornecedor: {F}",
            msg.NivelConfianca, msg.FornecedorId);
    }
}
