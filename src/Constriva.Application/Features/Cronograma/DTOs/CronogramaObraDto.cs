namespace Constriva.Application.Features.Cronograma.DTOs;

public record CronogramaObraDto(
    Guid Id, Guid ObraId, string ObraNome,
    DateTime DataInicio, DateTime DataFim,
    decimal PercentualPrevisto, decimal PercentualRealizado,
    IEnumerable<AtividadeDto> Atividades
);
