using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record UpdateCronogramaCommand(
    Guid Id,
    Guid EmpresaId,
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string? Descricao,
    string? Observacoes,
    bool ELinhaDBase,
    bool Ativo)
    : IRequest<CronogramaObraDto>, ITenantRequest;

public class UpdateCronogramaCommandHandler : IRequestHandler<UpdateCronogramaCommand, CronogramaObraDto>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateCronogramaCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<CronogramaObraDto> Handle(UpdateCronogramaCommand request, CancellationToken cancellationToken)
    {
        var cronograma = await _repo.GetByIdDetalhadoAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Cronograma não encontrado.");

        cronograma.Nome        = request.Nome;
        cronograma.DataInicio  = request.DataInicio;
        cronograma.DataFim     = request.DataFim;
        cronograma.Descricao   = request.Descricao;
        cronograma.Observacoes = request.Observacoes;
        cronograma.ELinhaDBase = request.ELinhaDBase;
        cronograma.Ativo       = request.Ativo;

        await _uow.SaveChangesAsync(cancellationToken);

        var atividades = cronograma.Atividades.Where(a => !a.IsDeleted).ToList();
        var percentualRealizado = atividades.Count > 0
            ? atividades.Average(a => a.PercentualConcluido)
            : 0m;

        var duracaoTotal = (cronograma.DataFim - cronograma.DataInicio).TotalDays;
        var diasDecorridos = (DateTime.UtcNow - cronograma.DataInicio).TotalDays;
        var percentualPrevisto = duracaoTotal > 0
            ? Math.Clamp((decimal)(diasDecorridos / duracaoTotal) * 100m, 0m, 100m)
            : 0m;

        return new CronogramaObraDto(
            cronograma.Id, cronograma.ObraId, cronograma.Nome, cronograma.Obra?.Nome ?? "",
            cronograma.DataInicio, cronograma.DataFim,
            Math.Round(percentualPrevisto, 2), Math.Round(percentualRealizado, 2),
            atividades.Select(a => new AtividadeDto(
                a.Id, a.Nome, a.Descricao, a.Ordem,
                a.DataInicioPlanejada, a.DataFimPlanejada,
                a.DataInicioReal, a.DataFimReal,
                a.PercentualConcluido, a.Status, a.NoCaminhosCritico,
                a.Predecessoras?.Select(p => p.AtividadeOrigemId) ?? [])));
    }
}
