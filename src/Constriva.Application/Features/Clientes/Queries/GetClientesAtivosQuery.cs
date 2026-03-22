using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Clientes.Queries;

public record GetClientesAtivosQuery(Guid EmpresaId)
    : IRequest<IEnumerable<ClienteAtivoDto>>, ITenantRequest;

public class GetClientesAtivosHandler : IRequestHandler<GetClientesAtivosQuery, IEnumerable<ClienteAtivoDto>>
{
    private readonly IClienteRepository _repo;
    public GetClientesAtivosHandler(IClienteRepository repo) => _repo = repo;

    public async Task<IEnumerable<ClienteAtivoDto>> Handle(GetClientesAtivosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetAllActiveByEmpresaAsync(r.EmpresaId, ct);
        return items.Select(c => new ClienteAtivoDto(c.Id, c.Nome));
    }
}
