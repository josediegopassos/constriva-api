using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetGruposOrcamentoQuery(
    Guid OrcamentoId,
    Guid EmpresaId) : IRequest<IEnumerable<GrupoOrcamentoDto>>, ITenantRequest;

public class GetGruposOrcamentoHandler
    : IRequestHandler<GetGruposOrcamentoQuery, IEnumerable<GrupoOrcamentoDto>>
{
    private readonly IGrupoOrcamentoRepository _repo;

    public GetGruposOrcamentoHandler(IGrupoOrcamentoRepository repo) => _repo = repo;

    public async Task<IEnumerable<GrupoOrcamentoDto>> Handle(
        GetGruposOrcamentoQuery request, CancellationToken ct)
    {
        var grupos = await _repo.GetByOrcamentoAsync(request.OrcamentoId, ct);
        return grupos
            .Where(g => !g.IsDeleted && g.GrupoPaiId == null)
            .OrderBy(g => g.Ordem)
            .Select(OrcamentoMapper.ToGrupoDto);
    }
}

