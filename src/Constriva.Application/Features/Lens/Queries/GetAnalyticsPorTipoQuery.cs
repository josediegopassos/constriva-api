using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Extensions;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetAnalyticsPorTipoQuery(
    Guid EmpresaId,
    DateTime? De = null,
    DateTime? Ate = null)
    : IRequest<List<AnalyticsPorTipoDto>>, ITenantRequest;

public class GetAnalyticsPorTipoHandler : IRequestHandler<GetAnalyticsPorTipoQuery, List<AnalyticsPorTipoDto>>
{
    private readonly IDocumentoLensRepository _repo;

    public GetAnalyticsPorTipoHandler(IDocumentoLensRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<AnalyticsPorTipoDto>> Handle(GetAnalyticsPorTipoQuery r, CancellationToken ct)
    {
        var grupos = await _repo.GetPorTipoAsync(r.EmpresaId, r.De, r.Ate, ct);

        return grupos.Select(g => new AnalyticsPorTipoDto(
            g.TipoDocumento.ToDescricao(),
            g.Total,
            g.Sucesso,
            g.Erro,
            g.ConfidenceMedio)).ToList();
    }
}
