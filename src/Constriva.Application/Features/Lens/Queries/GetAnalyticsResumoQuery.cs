using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetAnalyticsResumoQuery(
    Guid EmpresaId,
    DateTime? De = null,
    DateTime? Ate = null)
    : IRequest<ResumoAnalyticsLensDto>, ITenantRequest;

public class GetAnalyticsResumoHandler : IRequestHandler<GetAnalyticsResumoQuery, ResumoAnalyticsLensDto>
{
    private readonly IDocumentoLensRepository _repo;

    public GetAnalyticsResumoHandler(IDocumentoLensRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResumoAnalyticsLensDto> Handle(GetAnalyticsResumoQuery r, CancellationToken ct)
    {
        var de = r.De ?? DateTime.UtcNow.AddMonths(-3);
        var ate = r.Ate ?? DateTime.UtcNow;

        var (total, sucesso, erro, confidenceMedio, tempoMedio, totalItens, totalConsolidados) =
            await _repo.GetResumoAsync(r.EmpresaId, de, ate, ct);

        var taxaSucesso = total > 0 ? (float)sucesso / total * 100f : 0f;

        return new ResumoAnalyticsLensDto(
            total,
            sucesso,
            erro,
            taxaSucesso,
            confidenceMedio,
            tempoMedio,
            totalItens,
            totalConsolidados,
            de,
            ate);
    }
}
