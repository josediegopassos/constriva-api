using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetCurvaSQuery(Guid ObraId, Guid EmpresaId) : IRequest<IEnumerable<CurvaSPontoDto>>, ITenantRequest;

public class GetCurvaSHandler : IRequestHandler<GetCurvaSQuery, IEnumerable<CurvaSPontoDto>>
{
    private readonly ICronogramaRepository _repo;
    public GetCurvaSHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<IEnumerable<CurvaSPontoDto>> Handle(GetCurvaSQuery request, CancellationToken ct)
    {
        var crono = await _repo.GetByObraAsync(request.ObraId, request.EmpresaId, ct);
        if (crono == null) return [];
        var pontos = await _repo.GetCurvaSAsync(crono.Id, request.EmpresaId, ct);
        return pontos.Select(p => new CurvaSPontoDto(p.DataReferencia, p.PercentualPrevisto, p.PercentualRealizado));
    }
}
