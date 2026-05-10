using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.API.Consumers.WhatsApp;

public class WhatsAppPropostaFalhaConsumer(
    IHubContext<CotacaoWhatsAppHub> hub,
    ILogger<WhatsAppPropostaFalhaConsumer> logger)
    : WhatsAppConsumerBase(hub), IConsumer<PropostaExtraidaComFalhaEvent>
{
    public async Task Consume(ConsumeContext<PropostaExtraidaComFalhaEvent> context)
    {
        var msg = context.Message;
        await NotificarGrupoAsync(msg.CotacaoId, msg.EmpresaId, new
        {
            tipo = "proposta_extraida_com_falha",
            msg.FornecedorCotacaoId, msg.FornecedorId,
            motivo = msg.Motivo.ToString(), msg.MensagemParaGestor,
            msg.NivelConfiancaObtido, msg.RequerIntervencaoManual, msg.FalhouEm
        }, context.CancellationToken);

        logger.LogWarning("SignalR: extração falhou. Motivo: {M} | Fornecedor: {F}",
            msg.Motivo, msg.FornecedorId);
    }
}
