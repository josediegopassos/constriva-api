using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constriva.Infrastructure.Persistence;
using Constriva.Domain.Enums;
using Constriva.Messaging.Contracts.Lens.Events;
using Constriva.API.Hubs;

namespace Constriva.API.Consumers;

public class DocumentoLensErroConsumer : IConsumer<DocumentoLensErrorEvent>
{
    private readonly AppDbContext _db;
    private readonly ILensNotificationService _notificacao;
    private readonly ILogger<DocumentoLensErroConsumer> _logger;

    public DocumentoLensErroConsumer(
        AppDbContext db,
        ILensNotificationService notificacao,
        ILogger<DocumentoLensErroConsumer> logger)
    {
        _db = db;
        _notificacao = notificacao;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DocumentoLensErrorEvent> context)
    {
        var evento = context.Message;
        var ct = context.CancellationToken;

        _logger.LogError("Erro no processamento {ProcessamentoId}. Código: {Codigo}, Mensagem: {Mensagem}.",
            evento.ProcessamentoId, evento.CodigoErro, evento.MensagemErro);

        var documento = await _db.DocumentosLens
            .FirstOrDefaultAsync(d => d.Id == evento.ProcessamentoId && !d.IsDeleted, ct);

        if (documento is null)
        {
            _logger.LogWarning("Documento {ProcessamentoId} não encontrado no banco.", evento.ProcessamentoId);
            return;
        }

        documento.Status = StatusProcessamentoLensEnum.Erro;
        documento.CodigoErro = evento.CodigoErro;
        documento.MensagemErro = evento.MensagemErro;
        documento.PodeReprocessar = evento.PodeReprocessar;

        await _db.SaveChangesAsync(ct);

        await _notificacao.NotificarProcessamentoErro(
            documento, evento.CodigoErro, evento.MensagemErro, evento.PodeReprocessar);

        _logger.LogInformation("Status de erro atualizado para processamento {ProcessamentoId}.", evento.ProcessamentoId);
    }
}
