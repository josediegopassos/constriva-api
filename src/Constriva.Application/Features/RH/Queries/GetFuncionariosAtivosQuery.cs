using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetFuncionariosAtivosQuery(Guid EmpresaId) : IRequest<IEnumerable<FuncionarioResumoDto>>, ITenantRequest;

public class GetFuncionariosAtivosHandler : IRequestHandler<GetFuncionariosAtivosQuery, IEnumerable<FuncionarioResumoDto>>
{
    private readonly IRHRepository _repo;
    public GetFuncionariosAtivosHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<FuncionarioResumoDto>> Handle(GetFuncionariosAtivosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetFuncionariosAtivosAsync(r.EmpresaId, ct);
        return items.Select(f => new FuncionarioResumoDto(f.Id, f.Nome));
    }
}
