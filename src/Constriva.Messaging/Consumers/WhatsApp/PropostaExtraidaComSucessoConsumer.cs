using MassTransit;
using Microsoft.Extensions.Logging;
using Constriva.Messaging.Contracts.WhatsApp.Events;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class PropostaExtraidaComSucessoConsumer : IConsumer<PropostaExtraidaComSucessoEvent>
{
    private readonly ISignalRNotificationService _notificacao;
    private readonly ILogger<PropostaExtraidaComSucessoConsumer> _logger;

    public PropostaExtraidaComSucessoConsumer(
        ISignalRNotificationService notificacao, ILogger<PropostaExtraidaComSucessoConsumer> logger)
    { _notificacao = notificacao; _logger = logger; }

    public async Task Consume(ConsumeContext<PropostaExtraidaComSucessoEvent> context)
    {
        var msg = context.Message;
        await _notificacao.NotificarWhatsAppAtualizacaoAsync(
            msg.CotacaoId, msg.EmpresaId,
            new { tipo = "proposta_extraida_com_sucesso", msg.FornecedorCotacaoId,
                  msg.FornecedorId, msg.NivelConfianca, msg.CondicoesPagamento,
                  msg.PrazoEntregaDias, totalItens = msg.ItensExtraidos.Count, msg.ExtraidaEm },
            context.CancellationToken);

        _logger.LogInformation("SignalR: proposta extraída com sucesso. Confiança: {C} | Fornecedor: {F}",
            msg.NivelConfianca, msg.FornecedorId);
    }
}
