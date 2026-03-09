using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetPontosQuery(
    Guid EmpresaId, Guid? FuncionarioId = null, DateTime? Inicio = null, DateTime? Fim = null)
    : IRequest<IEnumerable<RegistroPontoDto>>, ITenantRequest;

public class GetPontosHandler : IRequestHandler<GetPontosQuery, IEnumerable<RegistroPontoDto>>
{
    private readonly IRHRepository _repo;
    public GetPontosHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<RegistroPontoDto>> Handle(GetPontosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetPontosAsync(r.EmpresaId, r.FuncionarioId, r.Inicio, r.Fim, ct);
        return items.Select(p => new RegistroPontoDto(
            p.Id, p.FuncionarioId, p.Funcionario?.Nome ?? "",
            p.Tipo, p.DataHora, p.HorarioPrevisto, p.HorasExtras));
    }
}
