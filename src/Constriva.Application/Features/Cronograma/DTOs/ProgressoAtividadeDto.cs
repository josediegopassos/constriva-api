namespace Constriva.Application.Features.Cronograma.DTOs;

public record ProgressoAtividadeDto(
    Guid Id, Guid AtividadeId, DateTime DataRegistro,
    decimal PercentualAnterior, decimal PercentualAtual,
    string? Observacoes, Guid RegistradoPor, string? FotoUrl
);
