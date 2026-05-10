using MassTransit;
using Microsoft.Extensions.Logging;
using Constriva.Messaging.Contracts.WhatsApp.Events;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class PropostaExtraidaComFalhaConsumer : IConsumer<PropostaExtraidaComFalhaEvent>
{
    private readonly ISignalRNotificationService _notificacao;
    private readonly ILogger<PropostaExtraidaComFalhaConsumer> _logger;

    public PropostaExtraidaComFalhaConsumer(
        ISignalRNotificationService notificacao, ILogger<PropostaExtraidaComFalhaConsumer> logger)
    { _notificacao = notificacao; _logger = logger; }

    public async Task Consume(ConsumeContext<PropostaExtraidaComFalhaEvent> context)
    {
        var msg = context.Message;
        await _notificacao.NotificarWhatsAppAtualizacaoAsync(
            msg.CotacaoId, msg.EmpresaId,
            new { tipo = "proposta_extraida_com_falha", msg.FornecedorCotacaoId,
                  msg.FornecedorId, motivo = msg.Motivo.ToString(),
                  msg.MensagemParaGestor, msg.NivelConfiancaObtido,
                  msg.RequerIntervencaoManual, msg.FalhouEm },
            context.CancellationToken);

        _logger.LogWarning("SignalR: extração falhou. Motivo: {M} | Fornecedor: {F}",
            msg.Motivo, msg.FornecedorId);
    }
}
