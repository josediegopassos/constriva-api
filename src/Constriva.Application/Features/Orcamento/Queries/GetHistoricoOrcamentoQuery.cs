using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetHistoricoOrcamentoQuery(
    Guid OrcamentoId,
    Guid EmpresaId) : IRequest<IEnumerable<HistoricoOrcamentoDto>>, ITenantRequest;

public class GetHistoricoOrcamentoHandler
    : IRequestHandler<GetHistoricoOrcamentoQuery, IEnumerable<HistoricoOrcamentoDto>>
{
    private readonly IOrcamentoRepository _repo;

    public GetHistoricoOrcamentoHandler(IOrcamentoRepository repo) => _repo = repo;

    public async Task<IEnumerable<HistoricoOrcamentoDto>> Handle(
        GetHistoricoOrcamentoQuery request, CancellationToken ct)
    {
        var historicos = await _repo.GetHistoricoAsync(request.OrcamentoId, request.EmpresaId, ct);
        return historicos.Select(h => new HistoricoOrcamentoDto(
            h.Id, h.Descricao, h.ValorAnterior, h.ValorNovo, h.UsuarioId, h.CreatedAt));
    }
}

