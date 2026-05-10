using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.WhatsApp.Commands;

public record AtualizarPropostaExtraidaCommand : IRequest<Guid>
{
    public required Guid EmpresaId { get; init; }
    public required Guid RespostaFornecedorWhatsAppId { get; init; }
    public required Guid CotacaoWhatsAppId { get; init; }
    public required Guid FornecedorCotacaoId { get; init; }
    public required Guid FornecedorId { get; init; }
    public required string WaMessageId { get; init; }
    public required int NivelConfianca { get; init; }
    public string? CondicoesPagamento { get; init; }
    public int? PrazoEntregaDias { get; init; }
    public DateTime? ValidadeProposta { get; init; }
    public string? Observacoes { get; init; }
    public required IReadOnlyList<ItemPropostaExtraidoContractDto> ItensExtraidos { get; init; }
}

public record ItemPropostaExtraidoContractDto
{
    public required string DescricaoOriginal { get; init; }
    public Guid? ItemCotacaoId { get; init; }
    public required decimal PrecoUnitario { get; init; }
    public required decimal Quantidade { get; init; }
    public string? Marca { get; init; }
    public bool? Disponivel { get; init; }
    public string? Observacao { get; init; }
}

public class AtualizarPropostaExtraidaHandler : IRequestHandler<AtualizarPropostaExtraidaCommand, Guid>
{
    private readonly IWhatsAppCotacaoRepository _repo;
    private readonly IComprasRepository _comprasRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<AtualizarPropostaExtraidaHandler> _logger;

    public AtualizarPropostaExtraidaHandler(
        IWhatsAppCotacaoRepository repo, IComprasRepository comprasRepo,
        IUnitOfWork uow, ILogger<AtualizarPropostaExtraidaHandler> logger)
    {
        _repo = repo;
        _comprasRepo = comprasRepo;
        _uow = uow;
        _logger = logger;
    }

    public async Task<Guid> Handle(AtualizarPropostaExtraidaCommand request, CancellationToken ct)
    {
        var resposta = await _repo.GetRespostaByIdAsync(
            request.RespostaFornecedorWhatsAppId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Resposta não encontrada");

        var cotacaoWa = await _repo.GetCotacaoWhatsAppByCotacaoIdAsync(
            request.CotacaoWhatsAppId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("CotacaoWhatsApp não encontrada");

        var proposta = new PropostaCotacao
        {
            EmpresaId = request.EmpresaId,
            CotacaoId = cotacaoWa.CotacaoId,
            FornecedorId = request.FornecedorId,
            DataRecebimento = resposta.RecebidaEm,
            DataValidade = request.ValidadeProposta,
            CondicoesPagamento = request.CondicoesPagamento,
            PrazoEntrega = request.PrazoEntregaDias,
            Observacoes = request.Observacoes,
            ValorTotal = request.ItensExtraidos.Sum(i => i.PrecoUnitario * i.Quantidade),
            Vencedora = false
        };

        proposta.Itens = request.ItensExtraidos
            .Where(i => i.ItemCotacaoId.HasValue)
            .Select(i => new ItemPropostaCotacao
            {
                EmpresaId = request.EmpresaId,
                PropostaId = proposta.Id,
                ItemCotacaoId = i.ItemCotacaoId!.Value,
                PrecoUnitario = i.PrecoUnitario,
                Quantidade = i.Quantidade,
                Marca = i.Marca,
                Observacao = i.Observacao,
                Disponivel = i.Disponivel ?? true,
                MenorPreco = false
            })
            .ToList();

        await _comprasRepo.AddPropostaAsync(proposta, ct);

        resposta.MarcarComoExtraidaComSucesso(proposta.Id, request.NivelConfianca);
        cotacaoWa.IncrementarPropostasExtraidas();

        await _uow.SaveChangesAsync(ct);
        await _repo.RecalcularMenorPrecoAsync(cotacaoWa.CotacaoId, request.EmpresaId, ct);

        _logger.LogInformation(
            "PropostaCotacao {PropostaId} criada com {TotalItens} itens. Confiança: {Confianca}",
            proposta.Id, proposta.Itens.Count, request.NivelConfianca);

        return proposta.Id;
    }
}

public class AtualizarPropostaExtraidaCommandValidator
    : AbstractValidator<AtualizarPropostaExtraidaCommand>
{
    public AtualizarPropostaExtraidaCommandValidator()
    {
        RuleFor(x => x.EmpresaId).NotEmpty();
        RuleFor(x => x.RespostaFornecedorWhatsAppId).NotEmpty();
        RuleFor(x => x.CotacaoWhatsAppId).NotEmpty();
        RuleFor(x => x.FornecedorId).NotEmpty();
        RuleFor(x => x.WaMessageId).NotEmpty();
        RuleFor(x => x.NivelConfianca).InclusiveBetween(30, 100)
            .WithMessage("NivelConfianca deve ser entre 30 e 100");
        RuleFor(x => x.ItensExtraidos).NotEmpty()
            .WithMessage("Proposta deve ter pelo menos 1 item");
        RuleForEach(x => x.ItensExtraidos).ChildRules(item =>
        {
            item.RuleFor(i => i.DescricaoOriginal).NotEmpty();
            item.RuleFor(i => i.PrecoUnitario).GreaterThan(0);
            item.RuleFor(i => i.Quantidade).GreaterThan(0);
        });
    }
}
