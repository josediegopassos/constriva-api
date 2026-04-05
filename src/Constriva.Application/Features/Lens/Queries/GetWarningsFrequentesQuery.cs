using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetWarningsFrequentesQuery(
    Guid EmpresaId,
    int Limite = 10)
    : IRequest<List<WarningFrequenteDto>>, ITenantRequest;

public class GetWarningsFrequentesHandler : IRequestHandler<GetWarningsFrequentesQuery, List<WarningFrequenteDto>>
{
    private readonly IDocumentoLensRepository _repo;

    public GetWarningsFrequentesHandler(IDocumentoLensRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<WarningFrequenteDto>> Handle(GetWarningsFrequentesQuery r, CancellationToken ct)
    {
        var warnings = await _repo.GetWarningsFrequentesAsync(r.EmpresaId, r.Limite, ct);

        return warnings.Select(w => new WarningFrequenteDto(w.Warning, w.Frequencia)).ToList();
    }
}
