namespace Constriva.Application.Features.Obras.DTOs;

public record FaseObraDto(
    Guid Id, string Nome, int Ordem, decimal PercentualConcluido,
    DateTime DataInicioPrevista, DateTime DataFimPrevista,
    decimal ValorPrevisto, Guid? FasePaiId);
