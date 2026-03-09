using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetOrcamentosByObraQuery(
    Guid ObraId,
    Guid EmpresaId) : IRequest<IEnumerable<OrcamentoResumoDto>>, ITenantRequest;

public class GetOrcamentosByObraHandler
    : IRequestHandler<GetOrcamentosByObraQuery, IEnumerable<OrcamentoResumoDto>>
{
    private readonly IOrcamentoRepository _repo;

    public GetOrcamentosByObraHandler(IOrcamentoRepository repo) => _repo = repo;

    public async Task<IEnumerable<OrcamentoResumoDto>> Handle(
        GetOrcamentosByObraQuery request, CancellationToken ct)
    {
        var orcamentos = await _repo.GetByObraAsync(request.ObraId, request.EmpresaId, ct);
        return orcamentos.Select(o =>
        {
            var totalGrupos = o.Grupos?.Count(g => !g.IsDeleted) ?? 0;
            var totalItens = o.Grupos?.SelectMany(g => g.Itens ?? []).Count(i => !i.IsDeleted) ?? 0;
            return OrcamentoMapper.ToResumoDto(o, totalGrupos);
        });
    }
}

