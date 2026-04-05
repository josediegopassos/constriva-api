using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetTendenciaConfidenceQuery(
    Guid EmpresaId,
    DateTime? De = null,
    DateTime? Ate = null)
    : IRequest<List<TendenciaConfidenceDto>>, ITenantRequest;

public class GetTendenciaConfidenceHandler : IRequestHandler<GetTendenciaConfidenceQuery, List<TendenciaConfidenceDto>>
{
    private readonly IDocumentoLensRepository _repo;

    public GetTendenciaConfidenceHandler(IDocumentoLensRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<TendenciaConfidenceDto>> Handle(GetTendenciaConfidenceQuery r, CancellationToken ct)
    {
        var tendencia = await _repo.GetTendenciaConfidenceAsync(r.EmpresaId, r.De, r.Ate, ct);

        return tendencia.Select(t => new TendenciaConfidenceDto(
            t.Data,
            t.ConfidenceMedio,
            t.TotalDocumentos)).ToList();
    }
}
