using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetDepartamentosQuery(Guid EmpresaId) : IRequest<IEnumerable<DepartamentoDto>>, ITenantRequest;

public class GetDepartamentosHandler : IRequestHandler<GetDepartamentosQuery, IEnumerable<DepartamentoDto>>
{
    private readonly IRHRepository _repo;
    public GetDepartamentosHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<DepartamentoDto>> Handle(GetDepartamentosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetDepartamentosAsync(r.EmpresaId, ct);
        return items.Select(d => new DepartamentoDto(d.Id, d.Nome, d.Descricao, d.GestorId, d.Gestor?.Nome, d.DepartamentoPaiId, d.Ativo));
    }
}
