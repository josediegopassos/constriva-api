using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Queries;

public record GetConsumoDiarioQuery(Guid EmpresaId)
    : IRequest<IEnumerable<ConsumoDiarioDto>>, ITenantRequest;

public class GetConsumoDiarioHandler : IRequestHandler<GetConsumoDiarioQuery, IEnumerable<ConsumoDiarioDto>>
{
    private readonly IAgenteRepository _repo;
    public GetConsumoDiarioHandler(IAgenteRepository repo) => _repo = repo;

    public async Task<IEnumerable<ConsumoDiarioDto>> Handle(GetConsumoDiarioQuery r, CancellationToken ct)
    {
        var items = await _repo.GetConsumoDiarioUltimos30DiasAsync(r.EmpresaId, ct);

        return items.Select(d => new ConsumoDiarioDto(
            d.Data, d.TokensInput + d.TokensOutput, d.TotalRequisicoes));
    }
}
