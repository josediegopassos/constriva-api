using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetCronogramaQuery(Guid? ObraId, Guid EmpresaId) : IRequest<IEnumerable<CronogramaObraDto>>, ITenantRequest;

public class GetCronogramaHandler : IRequestHandler<GetCronogramaQuery, IEnumerable<CronogramaObraDto>>
{
    private readonly ICronogramaRepository _repo;
    public GetCronogramaHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<IEnumerable<CronogramaObraDto>> Handle(GetCronogramaQuery request, CancellationToken ct)
    {
        if (request.ObraId.HasValue)
        {
            var crono = await _repo.GetWithAtividadesAsync(request.ObraId.Value, request.EmpresaId, ct);
            if (crono == null) return [];

            return [MapToDto(crono)];
        }

        var cronogramas = await _repo.GetAllWithAtividadesAsync(request.EmpresaId, ct);
        return cronogramas.Select(MapToDto);
    }

    private static CronogramaObraDto MapToDto(CronogramaObra crono)
    {
        var atividades = crono.Atividades.ToList();

        var percentualRealizado = atividades.Count > 0
            ? atividades.Average(a => a.PercentualConcluido)
            : 0m;

        var duracaoTotal = (crono.DataFim - crono.DataInicio).TotalDays;
        var diasDecorridos = (DateTime.UtcNow - crono.DataInicio).TotalDays;
        var percentualPrevisto = duracaoTotal > 0
            ? Math.Clamp((decimal)(diasDecorridos / duracaoTotal) * 100m, 0m, 100m)
            : 0m;

        return new CronogramaObraDto(
            crono.Id, crono.ObraId, crono.Nome, crono.Obra?.Nome ?? "",
            crono.DataInicio, crono.DataFim,
            Math.Round(percentualPrevisto, 2), Math.Round(percentualRealizado, 2),
            atividades.Select(a => new AtividadeDto(
                a.Id, a.Nome, a.Descricao, a.Ordem,
                a.DataInicioPlanejada, a.DataFimPlanejada,
                a.DataInicioReal, a.DataFimReal,
                a.PercentualConcluido, a.Status, a.NoCaminhosCritico,
                a.Predecessoras?.Select(p => p.AtividadeOrigemId) ?? [])));
    }
}
