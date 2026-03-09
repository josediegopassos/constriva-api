using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST;

public record GetIndicadoresSSTQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<IndicadoresDashboardDto>, ITenantRequest;

public class GetIndicadoresSSTHandler : IRequestHandler<GetIndicadoresSSTQuery, IndicadoresDashboardDto>
{
    private readonly ISSTRepository _repo;
    public GetIndicadoresSSTHandler(ISSTRepository repo) => _repo = repo;

    public async Task<IndicadoresDashboardDto> Handle(GetIndicadoresSSTQuery r, CancellationToken ct)
    {
        var indicadores = await _repo.GetIndicadoresAsync(r.EmpresaId, r.ObraId, ct);
        var acidentes = await _repo.GetAcidentesAsync(r.EmpresaId, r.ObraId, ct);
        var acidentesList = acidentes.ToList();

        var ultimo = acidentesList.OrderByDescending(a => a.DataHoraAcidente).FirstOrDefault();
        var diasSemAcidente = ultimo != null
            ? Math.Max(0, (int)(DateTime.Today - ultimo.DataHoraAcidente.Date).TotalDays)
            : 0;

        return new IndicadoresDashboardDto(
            indicadores.TotalDDS,
            acidentesList.Count,
            acidentesList.Count(a => a.AfastamentoMedico),
            acidentesList.Sum(a => a.DiasAfastamento ?? 0),
            indicadores.TaxaFrequencia,
            indicadores.TaxaGravidade,
            diasSemAcidente);
    }
}
