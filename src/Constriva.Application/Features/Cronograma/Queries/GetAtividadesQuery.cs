using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetAtividadesQuery(Guid ObraId, Guid EmpresaId) : IRequest<IEnumerable<AtividadeDto>>, ITenantRequest;

public class GetAtividadesHandler : IRequestHandler<GetAtividadesQuery, IEnumerable<AtividadeDto>>
{
    private readonly ICronogramaRepository _repo;
    public GetAtividadesHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<IEnumerable<AtividadeDto>> Handle(GetAtividadesQuery request, CancellationToken ct)
    {
        var crono = await _repo.GetWithAtividadesAsync(request.ObraId, request.EmpresaId, ct);
        if (crono == null) return [];

        return crono.Atividades
            .OrderBy(a => a.Ordem)
            .Select(a => new AtividadeDto(
                a.Id, a.Nome, a.Descricao, a.Ordem,
                a.DataInicioPlanejada, a.DataFimPlanejada,
                a.DataInicioReal, a.DataFimReal,
                a.PercentualConcluido, a.Status, a.NoCaminhosCritico,
                a.Predecessoras?.Select(p => p.AtividadeOrigemId) ?? []));
    }
}
