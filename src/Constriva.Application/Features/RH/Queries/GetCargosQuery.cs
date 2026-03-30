using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetCargosQuery(Guid EmpresaId) : IRequest<IEnumerable<CargoDto>>, ITenantRequest;

public class GetCargosHandler : IRequestHandler<GetCargosQuery, IEnumerable<CargoDto>>
{
    private readonly IRHRepository _repo;
    public GetCargosHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<CargoDto>> Handle(GetCargosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetCargosAsync(r.EmpresaId, ct);
        return items.Select(c => new CargoDto(c.Id, c.Codigo, c.Nome, c.CBO, c.Descricao, c.SalarioBase, c.SalarioMaximo, c.Ativo));
    }
}
