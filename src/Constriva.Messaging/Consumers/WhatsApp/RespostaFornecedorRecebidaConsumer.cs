using MassTransit;
using Microsoft.Extensions.Logging;
using Constriva.Messaging.Contracts.WhatsApp.Events;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class RespostaFornecedorRecebidaConsumer : IConsumer<RespostaFornecedorRecebidaEvent>
{
    private readonly ISignalRNotificationService _notificacao;
    private readonly ILogger<RespostaFornecedorRecebidaConsumer> _logger;

    public RespostaFornecedorRecebidaConsumer(
        ISignalRNotificationService notificacao, ILogger<RespostaFornecedorRecebidaConsumer> logger)
    { _notificacao = notificacao; _logger = logger; }

    public async Task Consume(ConsumeContext<RespostaFornecedorRecebidaEvent> context)
    {
        var msg = context.Message;
        await _notificacao.NotificarWhatsAppAtualizacaoAsync(
            msg.CotacaoId, msg.EmpresaId,
            new { tipo = "resposta_fornecedor_recebida", msg.FornecedorCotacaoId,
                  msg.FornecedorId, msg.NomeFornecedor, msg.RecebidaEm,
                  tipoConteudo = msg.TipoConteudo.ToString(), msg.WaMessageId },
            context.CancellationToken);

        _logger.LogInformation("SignalR: resposta recebida de {F}", msg.NomeFornecedor);
    }
}
