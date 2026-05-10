using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Infrastructure.Integrations.WhatsApp;
using Constriva.Infrastructure.Persistence;
using Constriva.Messaging.Contracts.WhatsApp.Commands;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class EnviarCotacaoWhatsAppConsumer : IConsumer<EnviarCotacaoWhatsAppCommand>
{
    private readonly IWhatsAppGateway _gateway;
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publish;
    private readonly ILogger<EnviarCotacaoWhatsAppConsumer> _logger;

    public EnviarCotacaoWhatsAppConsumer(
        IWhatsAppGateway gateway, AppDbContext db,
        IPublishEndpoint publish, ILogger<EnviarCotacaoWhatsAppConsumer> logger)
    {
        _gateway = gateway;
        _db = db;
        _publish = publish;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EnviarCotacaoWhatsAppCommand> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        var existente = await _db.MensagensWhatsApp.FirstOrDefaultAsync(m =>
            m.FornecedorCotacaoId == msg.FornecedorCotacaoId &&
            m.EmpresaId == msg.EmpresaId &&
            m.TipoMensagem == TipoMensagemWhatsAppEnum.ConviteCotacao && !m.IsDeleted, ct);

        if (existente != null && existente.Status != StatusEnvioWhatsAppEnum.Pendente)
        {
            _logger.LogWarning(
                "Cotação {NumeroCotacao} para {Fornecedor} já processada (Status: {Status}). Ignorando duplicata.",
                msg.NumeroCotacao, msg.NomeFornecedor, existente.Status);
            return;
        }

        _logger.LogInformation(
            "Enviando cotação {NumeroCotacao} para {Fornecedor} [Tel: {Telefone}]",
            msg.NumeroCotacao, msg.NomeFornecedor, Redact(msg.TelefoneWhatsApp));

        string waMessageId;
        try
        {
            waMessageId = await _gateway.EnviarConviteCotacaoAsync(
                msg.TelefoneWhatsApp, msg.NomeFornecedor, msg.NumeroCotacao,
                msg.TituloCotacao, msg.DataLimiteResposta, msg.UrlFormulario, ct);
        }
        catch (WhatsAppGatewayException ex)
        {
            var erroDetalhado = string.IsNullOrEmpty(ex.ResponseBody)
                ? ex.Message
                : $"{ex.Message} | Response: {ex.ResponseBody}";

            _logger.LogError(ex,
                "Falha ao enviar WhatsApp para {Fornecedor}. HttpStatus: {Status} | ResponseBody: {ResponseBody}",
                msg.NomeFornecedor, ex.HttpStatusCode, ex.ResponseBody);

            var falhou = await _db.MensagensWhatsApp.FirstOrDefaultAsync(m =>
                m.FornecedorCotacaoId == msg.FornecedorCotacaoId &&
                m.EmpresaId == msg.EmpresaId &&
                m.TipoMensagem == TipoMensagemWhatsAppEnum.ConviteCotacao && !m.IsDeleted, ct);
            if (falhou != null)
            {
                falhou.MarcarComoFalhou(erroDetalhado);
                await _db.SaveChangesAsync(ct);
            }

            await _publish.Publish(new WhatsAppEnvioFalhouEvent
            {
                CotacaoId = msg.CotacaoId, EmpresaId = msg.EmpresaId,
                FornecedorCotacaoId = msg.FornecedorCotacaoId, FornecedorId = msg.FornecedorId,
                NomeFornecedor = msg.NomeFornecedor, Erro = erroDetalhado,
                FalhouEm = DateTime.UtcNow
            }, ct);

            if (ex.HttpStatusCode is >= 400 and < 500)
                return;

            throw;
        }

        var mensagem = await _db.MensagensWhatsApp.FirstOrDefaultAsync(m =>
            m.FornecedorCotacaoId == msg.FornecedorCotacaoId &&
            m.EmpresaId == msg.EmpresaId &&
            m.TipoMensagem == TipoMensagemWhatsAppEnum.ConviteCotacao && !m.IsDeleted, ct);

        if (mensagem != null)
        {
            mensagem.MarcarComoEnviada(waMessageId);
            await _db.SaveChangesAsync(ct);
        }

        await _publish.Publish(new CotacaoEnviadaFornecedorEvent
        {
            CotacaoId = msg.CotacaoId,
            FornecedorCotacaoId = msg.FornecedorCotacaoId,
            FornecedorId = msg.FornecedorId,
            EmpresaId = msg.EmpresaId,
            WaMessageId = waMessageId,
            EnviadaEm = DateTime.UtcNow,
            Lembrete = false
        }, ct);

        _ = _gateway.MarcarMensagemComoLidaAsync(waMessageId, ct);

        _logger.LogInformation(
            "WhatsApp enviado. WaMessageId: {WaMessageId} | Fornecedor: {Fornecedor}",
            waMessageId, msg.NomeFornecedor);
    }

    private static string Redact(string tel) => tel.Length > 4 ? $"***{tel[^4..]}" : "****";
}
