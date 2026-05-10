using MediatR;
using MassTransit;
using FluentValidation;
using Microsoft.Extensions.Logging;

using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.Application.Features.Compras.WhatsApp.Commands;

public record AprovarPropostaWhatsAppCommand(
    Guid EmpresaId,
    Guid UsuarioId,
    Guid CotacaoId,
    Guid PropostaCotacaoId)
    : IRequest<AprovarPropostaWhatsAppResult>, ITenantRequest;

public record AprovarPropostaWhatsAppResult
{
    public required Guid PropostaCotacaoId { get; init; }
    public required string NomeFornecedorVencedor { get; init; }
    public required decimal ValorTotalAprovado { get; init; }
    public required int TotalItensAprovados { get; init; }
}

public class AprovarPropostaWhatsAppHandler
    : IRequestHandler<AprovarPropostaWhatsAppCommand, AprovarPropostaWhatsAppResult>
{
    private readonly IWhatsAppCotacaoRepository _repo;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;
    private readonly ILogger<AprovarPropostaWhatsAppHandler> _logger;

    public AprovarPropostaWhatsAppHandler(
        IWhatsAppCotacaoRepository repo, IUnitOfWork uow, IBus bus,
        ILogger<AprovarPropostaWhatsAppHandler> logger)
    {
        _repo = repo;
        _uow = uow;
        _bus = bus;
        _logger = logger;
    }

    public async Task<AprovarPropostaWhatsAppResult> Handle(
        AprovarPropostaWhatsAppCommand request, CancellationToken ct)
    {
        var empresaId = request.EmpresaId;

        var proposta = await _repo.GetPropostaComRelacionamentosAsync(
            request.PropostaCotacaoId, request.CotacaoId, empresaId, ct)
            ?? throw new KeyNotFoundException("Proposta não encontrada");

        if (await _repo.ExistePropostaVencedoraAsync(request.CotacaoId, empresaId, ct))
            throw new ValidationException("Esta cotação já possui uma proposta aprovada");

        proposta.Vencedora = true;
        proposta.Cotacao.FornecedorVencedorId = proposta.FornecedorId;
        proposta.Cotacao.Status = StatusCotacaoEnum.Fechada;

        var cotacaoWa = await _repo.GetCotacaoWhatsAppByCotacaoIdAsync(request.CotacaoId, empresaId, ct);
        cotacaoWa?.Encerrar();

        await _uow.SaveChangesAsync(ct);

        var nomeFornecedor = proposta.Fornecedor.NomeFantasia ?? proposta.Fornecedor.RazaoSocial;
        var telefone = proposta.Fornecedor.Celular ?? proposta.Fornecedor.Telefone ?? "";

        await _bus.Publish(new CotacaoAprovadaEvent
        {
            CotacaoId = request.CotacaoId,
            PropostaCotacaoId = request.PropostaCotacaoId,
            FornecedorId = proposta.FornecedorId,
            ObraId = proposta.Cotacao.ObraId,
            EmpresaId = empresaId,
            AprovadoPorUsuarioId = request.UsuarioId,
            AprovadaEm = DateTime.UtcNow,
            NumeroCotacao = proposta.Cotacao.Numero,
            NomeFornecedor = nomeFornecedor,
            TelefoneFornecedor = telefone,
            ValorTotalAprovado = proposta.ValorTotal,
            CondicoesPagamento = proposta.CondicoesPagamento,
            PrazoEntregaDias = proposta.PrazoEntrega,
            DataEntregaPrevista = proposta.PrazoEntrega.HasValue
                ? DateTime.UtcNow.AddDays(proposta.PrazoEntrega.Value)
                : null,
            ItensAprovados = proposta.Itens.Select(i => new ItemAprovadoDto
            {
                ItemCotacaoId = i.ItemCotacaoId,
                ItemPropostaCotacaoId = i.Id,
                Descricao = i.ItemCotacao?.Descricao ?? "",
                UnidadeMedida = i.ItemCotacao?.UnidadeMedida ?? "",
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                ValorTotal = i.PrecoUnitario * i.Quantidade,
                Marca = i.Marca,
                MaterialId = i.ItemCotacao?.MaterialId
            }).ToList()
        }, ct);

        _logger.LogInformation(
            "Proposta {PropostaId} aprovada. Fornecedor: {Fornecedor} | Valor: {Valor} | CotacaoId: {CotacaoId}",
            proposta.Id, nomeFornecedor, proposta.ValorTotal, request.CotacaoId);

        return new AprovarPropostaWhatsAppResult
        {
            PropostaCotacaoId = proposta.Id,
            NomeFornecedorVencedor = nomeFornecedor,
            ValorTotalAprovado = proposta.ValorTotal,
            TotalItensAprovados = proposta.Itens.Count
        };
    }
}

public class AprovarPropostaWhatsAppCommandValidator
    : AbstractValidator<AprovarPropostaWhatsAppCommand>
{
    public AprovarPropostaWhatsAppCommandValidator()
    {
        RuleFor(x => x.CotacaoId).NotEmpty();
        RuleFor(x => x.PropostaCotacaoId).NotEmpty();
    }
}
