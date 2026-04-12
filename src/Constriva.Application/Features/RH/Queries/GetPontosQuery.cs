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
        var items = (await _repo.GetPontosAsync(r.EmpresaId, r.FuncionarioId, r.Inicio, r.Fim, ct)).ToList();

        var userIds = items
            .SelectMany(p => new[] { p.AprovadoPor, p.ReprovadoPor })
            .Where(id => id.HasValue).Select(id => id!.Value);
        var nomes = await _repo.GetUsuarioNomesAsync(userIds, ct);

        return items.Select(p => new RegistroPontoDto(
            p.Id, p.FuncionarioId, p.Funcionario?.Nome ?? "",
            p.ObraId, p.Tipo, p.DataHora,
            p.HorarioPrevisto, p.HorasExtras,
            p.Latitude, p.Longitude, p.Dispositivo,
            p.Online, p.Manual, p.Justificativa,
            p.StatusAprovacao, p.AprovadoPor,
            p.AprovadoPor.HasValue && nomes.TryGetValue(p.AprovadoPor.Value, out var an) ? an : null,
            p.ReprovadoPor,
            p.ReprovadoPor.HasValue && nomes.TryGetValue(p.ReprovadoPor.Value, out var rn) ? rn : null,
            p.CreatedAt));
    }
}
