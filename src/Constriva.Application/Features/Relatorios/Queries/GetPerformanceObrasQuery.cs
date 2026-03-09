using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Relatorios.DTOs;

namespace Constriva.Application.Features.Relatorios;

public record GetPerformanceObrasQuery(Guid EmpresaId)
    : IRequest<IEnumerable<PerformanceObraDto>>, ITenantRequest;

public class GetPerformanceObrasHandler : IRequestHandler<GetPerformanceObrasQuery, IEnumerable<PerformanceObraDto>>
{
    private readonly IRelatoriosRepository _repo;

    public GetPerformanceObrasHandler(IRelatoriosRepository repo) => _repo = repo;

    public async Task<IEnumerable<PerformanceObraDto>> Handle(GetPerformanceObrasQuery r, CancellationToken ct)
    {
        var obras = await _repo.GetObrasParaRelatorioAsync(r.EmpresaId, null, ct);
        var hoje = DateTime.Today;
        return obras.Where(o => o.Status == StatusObraEnum.EmAndamento || o.Status == StatusObraEnum.Concluida)
            .Select(o =>
            {
                var duracaoTotal = Math.Max((o.DataFimPrevista - o.DataInicioPrevista).TotalDays, 1);
                var decorrido = Math.Min((hoje - o.DataInicioPrevista).TotalDays, duracaoTotal);
                var percPlanejado = (decimal)(decorrido / duracaoTotal) * 100m;
                var percRealizado = o.PercentualConcluido;
                var idp = percPlanejado > 0 ? percRealizado / percPlanejado : 1m;
                var percFinanceiro = o.ValorContrato > 0 ? o.ValorRealizado / o.ValorContrato * 100m : 0m;
                var idc = percFinanceiro > 0 ? percRealizado / percFinanceiro : 1m;

                return new PerformanceObraDto(
                    o.Id, o.Codigo, o.Nome,
                    percRealizado, percFinanceiro,
                    Math.Round(idp, 2), Math.Round(idc, 2),
                    o.Status == StatusObraEnum.EmAndamento && o.DataFimPrevista < hoje);
            });
    }
}
