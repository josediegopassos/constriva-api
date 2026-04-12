namespace Constriva.Application.Features.Contratos.DTOs;

public record UpdateMedicaoDto(
    string Numero, DateTime DataInicio, DateTime DataFim, decimal ValorMedicao,
    decimal PercentualMedicao, string? Observacoes = null, string? ArquivoUrl = null);
