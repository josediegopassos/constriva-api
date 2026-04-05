using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdateStatusCotacaoCommand(
    Guid Id,
    Guid EmpresaId,
    StatusCotacaoEnum Status,
    Guid? FornecedorVencedorId = null,
    string? Observacoes = null) : IRequest, ITenantRequest;

public class UpdateStatusCotacaoHandler : IRequestHandler<UpdateStatusCotacaoCommand>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateStatusCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(UpdateStatusCotacaoCommand request, CancellationToken ct)
    {
        var cotacao = await _repo.GetCotacaoByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {request.Id} não encontrada.");

        var transicaoValida = (cotacao.Status, request.Status) switch
        {
            (StatusCotacaoEnum.Aberta, StatusCotacaoEnum.Fechada) => true,
            (StatusCotacaoEnum.Aberta, StatusCotacaoEnum.Cancelada) => true,
            (StatusCotacaoEnum.Fechada, StatusCotacaoEnum.Encerrada) => true,
            (StatusCotacaoEnum.Fechada, StatusCotacaoEnum.Cancelada) => true,
            _ => false
        };

        if (!transicaoValida)
            throw new InvalidOperationException(
                $"Transição de status '{cotacao.Status}' para '{request.Status}' não é permitida.");

        cotacao.Status = request.Status;

        if (request.Status == StatusCotacaoEnum.Fechada)
            cotacao.DataFechamento = DateTime.UtcNow;

        // Marcar fornecedor vencedor e proposta vencedora (Fechada ou Encerrada)
        if (request.FornecedorVencedorId.HasValue
            && (request.Status == StatusCotacaoEnum.Fechada || request.Status == StatusCotacaoEnum.Encerrada))
        {
            cotacao.FornecedorVencedorId = request.FornecedorVencedorId;

            if (cotacao.Propostas?.Any() == true)
            {
                foreach (var proposta in cotacao.Propostas.Where(p => !p.IsDeleted))
                {
                    proposta.Vencedora = proposta.FornecedorId == request.FornecedorVencedorId.Value;
                }
            }
        }

        if (request.Observacoes is not null)
            cotacao.Observacoes = request.Observacoes;

        await _uow.SaveChangesAsync(ct);
    }
}
