using MediatR;
using MassTransit;
using FluentValidation;
using Microsoft.Extensions.Logging;

using Constriva.Domain.Entities.WhatsApp;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Messaging.Contracts.WhatsApp.Commands;

namespace Constriva.Application.Features.Compras.WhatsApp.Commands;

public record EnviarCotacoesWhatsAppCommand(
    Guid EmpresaId,
    Guid UsuarioId,
    Guid CotacaoId,
    IReadOnlyList<Guid>? FornecedoresIds,
    string? MensagemPersonalizada,
    DateTime? DataLimiteResposta)
    : IRequest<EnviarCotacoesWhatsAppResult>, ITenantRequest;

public record EnviarCotacoesWhatsAppResult
{
    public required Guid CotacaoWhatsAppId { get; init; }
    public required int TotalConvitesDisparados { get; init; }
    public required IReadOnlyList<string> FornecedoresSemTelefone { get; init; }
}

public class EnviarCotacoesWhatsAppHandler
    : IRequestHandler<EnviarCotacoesWhatsAppCommand, EnviarCotacoesWhatsAppResult>
{
    private readonly IWhatsAppCotacaoRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;
    private readonly ILogger<EnviarCotacoesWhatsAppHandler> _logger;

    public EnviarCotacoesWhatsAppHandler(
        IWhatsAppCotacaoRepository repo, IUnitOfWork uow, IBus bus,
        ILogger<EnviarCotacoesWhatsAppHandler> logger)
    {
        _repo = repo;
        _uow = uow;
        _bus = bus;
        _logger = logger;
    }

    public async Task<EnviarCotacoesWhatsAppResult> Handle(
        EnviarCotacoesWhatsAppCommand request, CancellationToken ct)
    {
        var empresaId = request.EmpresaId;

        var cotacao = await _repo.GetCotacaoComFornecedoresEItensAsync(request.CotacaoId, empresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {request.CotacaoId} não encontrada");

        if (cotacao.Status is StatusCotacaoEnum.Fechada or StatusCotacaoEnum.Cancelada or StatusCotacaoEnum.Encerrada)
            throw new ValidationException("Cotação não está em estado válido para envio via WhatsApp");

        if (await _repo.ExisteCotacaoWhatsAppAsync(request.CotacaoId, empresaId, ct))
            throw new ValidationException("Esta cotação já possui uma sessão WhatsApp ativa");

        var fornecedores = request.FornecedoresIds is { Count: > 0 }
            ? cotacao.FornecedoresConvidados.Where(fc => request.FornecedoresIds.Contains(fc.Id)).ToList()
            : cotacao.FornecedoresConvidados.ToList();

        var comTelefone = fornecedores
            .Where(fc => !string.IsNullOrEmpty(fc.Fornecedor.Celular) || !string.IsNullOrEmpty(fc.Fornecedor.Telefone))
            .ToList();

        var semTelefone = fornecedores
            .Where(fc => string.IsNullOrEmpty(fc.Fornecedor.Celular) && string.IsNullOrEmpty(fc.Fornecedor.Telefone))
            .Select(fc => fc.Fornecedor.NomeFantasia ?? fc.Fornecedor.RazaoSocial)
            .ToList();

        if (comTelefone.Count == 0)
            throw new ValidationException("Nenhum fornecedor possui telefone cadastrado para WhatsApp");

        var dataLimite = request.DataLimiteResposta ?? cotacao.DataLimiteResposta ?? DateTime.UtcNow.AddDays(3);

        var cotacaoWa = new CotacaoWhatsApp(
            empresaId: empresaId,
            cotacaoId: request.CotacaoId,
            telefoneEmpresa: "",
            nomeExibicaoEmpresa: "Constriva",
            dataLimiteResposta: dataLimite,
            mensagemPersonalizada: request.MensagemPersonalizada);

        await _repo.AddCotacaoWhatsAppAsync(cotacaoWa, ct);

        var mensagens = comTelefone.Select(fc => new MensagemWhatsApp(
            empresaId: empresaId,
            cotacaoWhatsAppId: cotacaoWa.Id,
            fornecedorCotacaoId: fc.Id,
            fornecedorId: fc.FornecedorId,
            telefoneDestino: NormalizarTelefoneE164(fc.Fornecedor.Celular ?? fc.Fornecedor.Telefone!),
            nomeFornecedor: fc.Fornecedor.NomeFantasia ?? fc.Fornecedor.RazaoSocial,
            tipoMensagem: TipoMensagemWhatsAppEnum.ConviteCotacao,
            payloadEnviado: "{}")).ToList();

        await _repo.AddMensagensAsync(mensagens, ct);
        cotacaoWa.MarcarComoDisparada(comTelefone.Count);
        await _uow.SaveChangesAsync(ct);

        foreach (var fc in comTelefone)
        {
            var telefone = NormalizarTelefoneE164(fc.Fornecedor.Celular ?? fc.Fornecedor.Telefone!);
            await _bus.Publish(new EnviarCotacaoWhatsAppCommand
            {
                CotacaoId = cotacao.Id,
                FornecedorCotacaoId = fc.Id,
                FornecedorId = fc.FornecedorId,
                EmpresaId = empresaId,
                NumeroCotacao = cotacao.Numero,
                TituloCotacao = cotacao.Titulo,
                NomeFornecedor = fc.Fornecedor.NomeFantasia ?? fc.Fornecedor.RazaoSocial,
                TelefoneWhatsApp = telefone,
                DataLimiteResposta = dataLimite,
                UrlFormulario = $"/cotacoes/{cotacao.Id}/responder",
                Itens = cotacao.Itens.Select(i => new ItemCotacaoDto
                {
                    ItemCotacaoId = i.Id,
                    Descricao = i.Descricao,
                    UnidadeMedida = i.UnidadeMedida,
                    Quantidade = i.Quantidade,
                    Especificacao = i.Especificacao,
                    PrecoReferencia = i.PrecoReferencia
                }).ToList(),
                MensagemPersonalizada = request.MensagemPersonalizada
            }, ct);
        }

        _logger.LogInformation(
            "Cotação {NumeroCotacao} enviada via WhatsApp para {Total} fornecedores. CotacaoWhatsAppId: {Id}",
            cotacao.Numero, comTelefone.Count, cotacaoWa.Id);

        return new EnviarCotacoesWhatsAppResult
        {
            CotacaoWhatsAppId = cotacaoWa.Id,
            TotalConvitesDisparados = comTelefone.Count,
            FornecedoresSemTelefone = semTelefone
        };
    }

    private static string NormalizarTelefoneE164(string telefone)
    {
        var digits = new string(telefone.Where(char.IsDigit).ToArray());
        if (digits.StartsWith('0'))
            digits = digits[1..];
        if (!digits.StartsWith("55"))
            digits = "55" + digits;
        return "+" + digits;
    }
}

public class EnviarCotacoesWhatsAppCommandValidator : AbstractValidator<EnviarCotacoesWhatsAppCommand>
{
    public EnviarCotacoesWhatsAppCommandValidator()
    {
        RuleFor(x => x.CotacaoId).NotEmpty().WithMessage("CotacaoId é obrigatório");
        RuleFor(x => x.DataLimiteResposta)
            .GreaterThan(DateTime.UtcNow.AddHours(1))
            .When(x => x.DataLimiteResposta.HasValue)
            .WithMessage("DataLimiteResposta deve ser pelo menos 1 hora no futuro");
        RuleFor(x => x.MensagemPersonalizada)
            .MaximumLength(1000).When(x => x.MensagemPersonalizada != null);
    }
}
