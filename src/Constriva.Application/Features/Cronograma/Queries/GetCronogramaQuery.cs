using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetCronogramaQuery(Guid ObraId, Guid EmpresaId) : IRequest<CronogramaObraDto?>, ITenantRequest;

public class GetCronogramaHandler : IRequestHandler<GetCronogramaQuery, CronogramaObraDto?>
{
    private readonly ICronogramaRepository _repo;
    public GetCronogramaHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<CronogramaObraDto?> Handle(GetCronogramaQuery request, CancellationToken ct)
    {
        var crono = await _repo.GetWithAtividadesAsync(request.ObraId, request.EmpresaId, ct);
        if (crono == null) return null;

        return new CronogramaObraDto(
            crono.Id, crono.ObraId, "",
            crono.DataInicio, crono.DataFim,
            0m, 0m,
            crono.Atividades.Select(a => new AtividadeDto(
                a.Id, a.Nome, a.Descricao, a.Ordem,
                a.DataInicioPlanejada, a.DataFimPlanejada,
                a.DataInicioReal, a.DataFimReal,
                a.PercentualConcluido, a.Status, a.NoCaminhosCritico,
                a.Predecessoras?.Select(p => p.AtividadeOrigemId) ?? [])));
    }
}
