using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Obras.DTOs;

public record FaseObraDto(
    Guid Id,
    string Nome,
    string? Descricao,
    int Ordem,
    StatusFaseEnum Status,
    decimal PercentualConcluido,
    DateTime DataInicioPrevista,
    DateTime DataFimPrevista,
    DateTime? DataInicioReal,
    DateTime? DataFimReal,
    decimal ValorPrevisto,
    decimal ValorRealizado,
    Guid? FasePaiId,
    string? Cor);
