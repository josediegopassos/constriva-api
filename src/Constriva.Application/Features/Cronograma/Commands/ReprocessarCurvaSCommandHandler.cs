using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record ReprocessarCurvaSCommand(Guid CronogramaId, Guid EmpresaId)
    : IRequest<IEnumerable<CurvaSPontoDto>>, ITenantRequest;

public class ReprocessarCurvaSCommandHandler : IRequestHandler<ReprocessarCurvaSCommand, IEnumerable<CurvaSPontoDto>>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public ReprocessarCurvaSCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<IEnumerable<CurvaSPontoDto>> Handle(ReprocessarCurvaSCommand request, CancellationToken ct)
    {
        var crono = await _repo.GetByIdDetalhadoAsync(request.CronogramaId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Cronograma não encontrado.");

        var atividades = crono.Atividades.Where(a => !a.IsDeleted).ToList();
        if (atividades.Count == 0)
            throw new InvalidOperationException("Não há atividades no cronograma para gerar a Curva S.");

        await _repo.RemoveCurvaSAsync(crono.Id, request.EmpresaId, ct);

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

        // Gera datas semanais + inclui hoje se não coincidir com um ponto semanal
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

        var novosPontos = new List<CurvaSPonto>();
        var dtos = new List<CurvaSPontoDto>();

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

            previsto = Math.Round(previsto, 2);
            realizado = Math.Round(realizado, 2);

            novosPontos.Add(new CurvaSPonto
            {
                CronogramaId = crono.Id,
                EmpresaId = request.EmpresaId,
                DataReferencia = data,
                PercentualPrevisto = previsto,
                PercentualRealizado = realizado,
                ValorPrevisto = 0,
                ValorRealizado = 0
            });

            dtos.Add(new CurvaSPontoDto(data, previsto, realizado));
        }

        await _repo.AddCurvaSRangeAsync(novosPontos, ct);
        await _uow.SaveChangesAsync(ct);

        return dtos;
    }
}
