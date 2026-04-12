namespace Constriva.Application.Features.Contratos.DTOs;

public record MedicaoContratualDto(
    Guid Id, Guid ContratoId, string Numero, DateTime DataInicio, DateTime DataFim,
    decimal ValorMedicao, decimal PercentualMedicao, string? Observacoes, string? ArquivoUrl,
    DateTime CreatedAt);
