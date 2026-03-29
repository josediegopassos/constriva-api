using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetCurvaSQuery(Guid ObraId, Guid EmpresaId) : IRequest<IEnumerable<CurvaSPontoDto>>, ITenantRequest;

public class GetCurvaSHandler : IRequestHandler<GetCurvaSQuery, IEnumerable<CurvaSPontoDto>>
{
    private readonly ICronogramaRepository _repo;
    public GetCurvaSHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<IEnumerable<CurvaSPontoDto>> Handle(GetCurvaSQuery request, CancellationToken ct)
    {
        var crono = await _repo.GetWithAtividadesAsync(request.ObraId, request.EmpresaId, ct);
        if (crono == null) return [];

        var atividades = crono.Atividades.Where(a => !a.IsDeleted).ToList();
        if (atividades.Count == 0) return [];

        var dataInicio = crono.DataInicio.Date;
        var dataFim = crono.DataFim.Date;
        var hoje = DateTime.Today;
        var totalAtividades = (decimal)atividades.Count;

        var inicioReal = atividades
            .Where(a => a.DataInicioReal.HasValue)
            .Select(a => a.DataInicioReal!.Value.Date)
            .DefaultIfEmpty(dataInicio)
            .Min();
        var inicioEfetivo = inicioReal < dataInicio ? inicioReal : dataInicio;

        var datas = new SortedSet<DateTime>();
        var dataAtual = inicioEfetivo;
        while (dataAtual <= dataFim)
        {
            datas.Add(dataAtual);
            var proxima = dataAtual.AddDays(7);
            dataAtual = proxima > dataFim && dataAtual < dataFim ? dataFim : proxima;
        }
        if (hoje >= inicioEfetivo && hoje <= dataFim)
            datas.Add(hoje);

        var pontos = new List<CurvaSPontoDto>();

        foreach (var data in datas)
        {
            var previsto = atividades.Count(a => a.DataFimPlanejada.Date <= data) / totalAtividades * 100m;

            decimal realizado = 0m;
            if (data <= hoje)
            {
                realizado = atividades
                    .Where(a => (a.DataInicioReal?.Date ?? a.DataInicioPlanejada.Date) <= data)
                    .Sum(a => a.PercentualConcluido) / totalAtividades;
            }

            pontos.Add(new CurvaSPontoDto(
                data,
                Math.Round(previsto, 2),
                Math.Round(realizado, 2)));
        }

        return pontos;
    }
}
