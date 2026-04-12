using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetPontosPagedQuery(
    Guid EmpresaId, Guid? FuncionarioId = null, DateTime? Inicio = null,
    DateTime? Fim = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<RegistroPontoDto>>, ITenantRequest;

public class GetPontosPagedHandler : IRequestHandler<GetPontosPagedQuery, PaginatedResult<RegistroPontoDto>>
{
    private readonly IRHRepository _repo;
    public GetPontosPagedHandler(IRHRepository repo) => _repo = repo;

    public async Task<PaginatedResult<RegistroPontoDto>> Handle(GetPontosPagedQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPontosPagedAsync(
            r.EmpresaId, r.FuncionarioId, r.Inicio, r.Fim, r.Page, r.PageSize, ct);

        var userIds = items
            .SelectMany(p => new[] { p.AprovadoPor, p.ReprovadoPor })
            .Where(id => id.HasValue).Select(id => id!.Value);
        var nomes = await _repo.GetUsuarioNomesAsync(userIds, ct);

        return new PaginatedResult<RegistroPontoDto>
        {
            Items = items.Select(p => new RegistroPontoDto(
                p.Id, p.FuncionarioId, p.Funcionario?.Nome ?? "",
                p.ObraId, p.Tipo, p.DataHora,
                p.HorarioPrevisto, p.HorasExtras,
                p.Latitude, p.Longitude, p.Dispositivo,
                p.Online, p.Manual, p.Justificativa,
                p.StatusAprovacao, p.AprovadoPor,
                p.AprovadoPor.HasValue && nomes.TryGetValue(p.AprovadoPor.Value, out var an) ? an : null,
                p.ReprovadoPor,
                p.ReprovadoPor.HasValue && nomes.TryGetValue(p.ReprovadoPor.Value, out var rn) ? rn : null,
                p.CreatedAt)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
