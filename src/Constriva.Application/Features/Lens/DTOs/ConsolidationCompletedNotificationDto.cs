namespace Constriva.Application.Features.Lens.DTOs;

public record ConsolidationCompletedNotificationDto(
    Guid ProcessamentoId,
    Guid CompraId,
    int TotalItensConsolidados,
    int TotalItensRejeitados,
    decimal ValorTotal,
    string Mensagem);
