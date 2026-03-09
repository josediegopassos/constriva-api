namespace Constriva.Application.Features.Relatorios.DTOs;

public record PerformanceObraDto(
    Guid ObraId, string ObraCode, string ObraNome,
    decimal PercentualFisico, decimal PercentualFinanceiro,
    decimal IDP, decimal IDC, bool Atrasada);
