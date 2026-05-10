using MediatR;
using MassTransit;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Constriva.Domain.Entities.WhatsApp;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Messaging.Contracts.WhatsApp.Commands;

namespace Constriva.Application.Features.Compras.WhatsApp.Commands;

public record RegistrarRespostaFornecedorCommand : IRequest<RegistrarRespostaFornecedorResult>
{
    public required Guid EmpresaId { get; init; }
    public required string WaMessageId { get; init; }
    public required string TelefoneOrigem { get; init; }
    public required string TelefoneDestino { get; init; }
    public required DateTime RecebidaEm { get; init; }
    public required TipoConteudoMensagemEnum TipoConteudo { get; init; }
    public string? TextoMensagem { get; init; }
    public string? WaMediaId { get; init; }
    public string? MediaUrl { get; init; }
    public string? MediaMimeType { get; init; }
    public string? MediaNomeArquivo { get; init; }
    public required string PayloadWebhookOriginal { get; init; }
}

public record RegistrarRespostaFornecedorResult
{
    public required Guid RespostaFornecedorWhatsAppId { get; init; }
    public required Guid CotacaoId { get; init; }
    public required Guid FornecedorId { get; init; }
    public required bool NovaResposta { get; init; }
}

public class RegistrarRespostaFornecedorHandler
    : IRequestHandler<RegistrarRespostaFornecedorCommand, RegistrarRespostaFornecedorResult>
{
    private readonly IWhatsAppCotacaoRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;
    private readonly ILogger<RegistrarRespostaFornecedorHandler> _logger;

    public RegistrarRespostaFornecedorHandler(
        IWhatsAppCotacaoRepository repo, IUnitOfWork uow, IBus bus,
        ILogger<RegistrarRespostaFornecedorHandler> logger)
    {
        _repo = repo;
        _uow = uow;
        _bus = bus;
        _logger = logger;
    }

    public async Task<RegistrarRespostaFornecedorResult> Handle(
        RegistrarRespostaFornecedorCommand request, CancellationToken ct)
    {
        var existente = await _repo.GetRespostaByWaMessageIdAsync(request.WaMessageId, request.EmpresaId, ct);
        if (existente != null)
        {
            _logger.LogWarning("WaMessageId {WaMessageId} já registrado — ignorando duplicata", request.WaMessageId);
            return new RegistrarRespostaFornecedorResult
            {
                RespostaFornecedorWhatsAppId = existente.Id,
                CotacaoId = existente.CotacaoWhatsAppId,
                FornecedorId = existente.FornecedorId,
                NovaResposta = false
            };
        }

        var cotacaoWa = await _repo.GetCotacaoWhatsAppAtivaAsync(request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Nenhuma cotação WhatsApp ativa encontrada");

        var mensagemOriginal = await _repo.GetMensagemPorTelefoneAsync(
            cotacaoWa.Id, request.TelefoneOrigem, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException(
                $"Fornecedor com telefone {request.TelefoneOrigem} não está nesta cotação");

        var resposta = new RespostaFornecedorWhatsApp(
            empresaId: request.EmpresaId,
            cotacaoWhatsAppId: cotacaoWa.Id,
            fornecedorCotacaoId: mensagemOriginal.FornecedorCotacaoId,
            fornecedorId: mensagemOriginal.FornecedorId,
            waMessageId: request.WaMessageId,
            telefoneOrigem: request.TelefoneOrigem,
            recebidaEm: request.RecebidaEm,
            tipoConteudo: request.TipoConteudo,
            payloadWebhookOriginal: request.PayloadWebhookOriginal,
            textoMensagem: request.TextoMensagem,
            waMediaId: request.WaMediaId,
            mediaUrl: request.MediaUrl,
            mediaMimeType: request.MediaMimeType,
            mediaNomeArquivo: request.MediaNomeArquivo);

        await _repo.AddRespostaAsync(resposta, ct);
        cotacaoWa.IncrementarRespostas();
        mensagemOriginal.MarcarComoRespondida();
        await _uow.SaveChangesAsync(ct);

        await _bus.Publish(new ProcessarRespostaFornecedorCommand
        {
            EmpresaId = request.EmpresaId,
            WaMessageId = request.WaMessageId,
            TelefoneOrigem = request.TelefoneOrigem,
            TelefoneDestino = request.TelefoneDestino,
            RecebidaEm = request.RecebidaEm,
            TipoConteudo = (TipoConteudoWhatsApp)(int)request.TipoConteudo,
            TextoMensagem = request.TextoMensagem,
            MediaId = request.WaMediaId,
            MediaMimeType = request.MediaMimeType,
            MediaNomeArquivo = request.MediaNomeArquivo,
            PayloadWebhookOriginal = request.PayloadWebhookOriginal
        }, ct);

        _logger.LogInformation(
            "Resposta registrada. WaMessageId: {WaMessageId} | FornecedorId: {FornecedorId}",
            request.WaMessageId, mensagemOriginal.FornecedorId);

        return new RegistrarRespostaFornecedorResult
        {
            RespostaFornecedorWhatsAppId = resposta.Id,
            CotacaoId = cotacaoWa.CotacaoId,
            FornecedorId = mensagemOriginal.FornecedorId,
            NovaResposta = true
        };
    }
}

public class RegistrarRespostaFornecedorCommandValidator
    : AbstractValidator<RegistrarRespostaFornecedorCommand>
{
    public RegistrarRespostaFornecedorCommandValidator()
    {
        RuleFor(x => x.EmpresaId).NotEmpty();
        RuleFor(x => x.WaMessageId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TelefoneOrigem).NotEmpty().MaximumLength(20);
        RuleFor(x => x.TelefoneDestino).NotEmpty().MaximumLength(20);
        RuleFor(x => x.RecebidaEm).NotEmpty();
        RuleFor(x => x.TipoConteudo).IsInEnum();
        RuleFor(x => x.PayloadWebhookOriginal).NotEmpty();
        RuleFor(x => x.TextoMensagem).NotEmpty()
            .When(x => x.TipoConteudo == TipoConteudoMensagemEnum.Texto)
            .WithMessage("TextoMensagem é obrigatório quando TipoConteudo = Texto");
        RuleFor(x => x.WaMediaId).NotEmpty()
            .When(x => x.TipoConteudo is TipoConteudoMensagemEnum.Imagem or TipoConteudoMensagemEnum.Documento)
            .WithMessage("WaMediaId é obrigatório para mensagens com mídia");
    }
}
