using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetCronogramaByIdQuery(Guid Id, Guid EmpresaId) : IRequest<CronogramaDetalhadoDto?>, ITenantRequest;

public class GetCronogramaByIdHandler : IRequestHandler<GetCronogramaByIdQuery, CronogramaDetalhadoDto?>
{
    private readonly ICronogramaRepository _repo;
    public GetCronogramaByIdHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<CronogramaDetalhadoDto?> Handle(GetCronogramaByIdQuery request, CancellationToken ct)
    {
        var crono = await _repo.GetByIdDetalhadoAsync(request.Id, request.EmpresaId, ct);
        if (crono == null) return null;

        var atividades = crono.Atividades.Where(a => !a.IsDeleted).ToList();

        var percentualRealizado = atividades.Count > 0
            ? atividades.Average(a => a.PercentualConcluido)
            : 0m;

        var duracaoTotal = (crono.DataFim - crono.DataInicio).TotalDays;
        var diasDecorridos = (DateTime.UtcNow - crono.DataInicio).TotalDays;
        var percentualPrevisto = duracaoTotal > 0
            ? Math.Clamp((decimal)(diasDecorridos / duracaoTotal) * 100m, 0m, 100m)
            : 0m;

        return new CronogramaDetalhadoDto(
            crono.Id, crono.ObraId, crono.Nome, crono.Obra?.Nome ?? "",
            crono.Descricao, crono.Observacoes,
            crono.ELinhaDBase, crono.Versao, crono.VersaoBaseadaEm,
            crono.DataInicio, crono.DataFim,
            Math.Round(percentualPrevisto, 2), Math.Round(percentualRealizado, 2),
            atividades.Select(a => new AtividadeDetalhadaDto(
                a.Id, a.Codigo, a.Nome, a.Descricao,
                a.Nivel, a.Ordem, a.EAgrupadoa, a.EMarcador,
                a.Status, a.DuracaoDias > 0 ? a.DuracaoDias : (int)(a.DataFimPlanejada - a.DataInicioPlanejada).TotalDays, a.PercentualConcluido,
                a.DataInicioPlanejada, a.DataFimPlanejada,
                a.DataInicioReal, a.DataFimReal,
                a.DataInicioReprogramada, a.DataFimReprogramada,
                a.BCWS, a.BCWP, a.ACWP,
                a.CustoOrcado, a.CustoRealizado,
                a.NoCaminhosCritico, a.Folga,
                a.ResponsavelId, a.Cor, a.Observacoes,
                a.AtividadePaiId,
                a.Predecessoras?.Select(p => new VinculoAtividadeDto(p.Id, p.AtividadeOrigemId, p.AtividadeDestinoId, p.Tipo, p.Lag)) ?? [],
                a.Sucessoras?.Select(s => new VinculoAtividadeDto(s.Id, s.AtividadeOrigemId, s.AtividadeDestinoId, s.Tipo, s.Lag)) ?? [],
                a.Recursos?.Select(r => new RecursoAtividadeDto(r.Id, r.AtividadeId, r.TipoRecurso, r.NomeRecurso, r.RecursoId, r.Quantidade, r.UnidadeMedida, r.CustoUnitario, r.CustoTotal)) ?? [],
                []
            )));
    }
}
