using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.API.Consumers.WhatsApp;

public class WhatsAppRespostaRecebidaConsumer(
    IHubContext<CotacaoWhatsAppHub> hub,
    ILogger<WhatsAppRespostaRecebidaConsumer> logger)
    : WhatsAppConsumerBase(hub), IConsumer<RespostaFornecedorRecebidaEvent>
{
    public async Task Consume(ConsumeContext<RespostaFornecedorRecebidaEvent> context)
    {
        var msg = context.Message;
        await NotificarGrupoAsync(msg.CotacaoId, msg.EmpresaId, new
        {
            tipo = "resposta_fornecedor_recebida",
            msg.FornecedorCotacaoId, msg.FornecedorId,
            msg.NomeFornecedor, msg.RecebidaEm,
            tipoConteudo = msg.TipoConteudo.ToString(), msg.WaMessageId
        }, context.CancellationToken);

        logger.LogInformation("SignalR: resposta recebida de {F}", msg.NomeFornecedor);
    }
}
