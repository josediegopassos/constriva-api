namespace Constriva.Application.Features.Contratos.DTOs;

public record CreateMedicaoDto(
    string Numero, DateTime DataInicio, DateTime DataFim, decimal ValorMedicao,
    decimal PercentualMedicao, string? Observacoes = null);
