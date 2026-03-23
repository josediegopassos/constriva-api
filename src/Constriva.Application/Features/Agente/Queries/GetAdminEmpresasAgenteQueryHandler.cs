using MediatR;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Queries;

public record GetAdminEmpresasAgenteQuery()
    : IRequest<IEnumerable<AdminEmpresaAgenteDto>>;

public class GetAdminEmpresasAgenteHandler : IRequestHandler<GetAdminEmpresasAgenteQuery, IEnumerable<AdminEmpresaAgenteDto>>
{
    private readonly IAgenteRepository _repo;
    public GetAdminEmpresasAgenteHandler(IAgenteRepository repo) => _repo = repo;

    public async Task<IEnumerable<AdminEmpresaAgenteDto>> Handle(GetAdminEmpresasAgenteQuery r, CancellationToken ct)
    {
        var configs = await _repo.GetTodasEmpresasAtivasAsync(ct);
        var now = DateTime.UtcNow;

        var result = new List<AdminEmpresaAgenteDto>();
        foreach (var cfg in configs)
        {
            var consumo = await _repo.GetConsumoMensalAsync(cfg.EmpresaId, now.Year, now.Month, ct);

            result.Add(new AdminEmpresaAgenteDto(
                cfg.EmpresaId,
                cfg.EmpresaId.ToString(),
                cfg.Tier?.Nome ?? "Sem Tier",
                consumo?.TokensUtilizados ?? 0,
                cfg.Tier?.TokensMensais ?? 0,
                cfg.Ativo));
        }

        return result;
    }
}
