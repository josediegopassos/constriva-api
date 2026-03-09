namespace Constriva.Application.Features.Relatorios.DTOs;

public record DashboardEmpresaDto(
    int TotalObras, int ObrasEmAndamento, int ObrasAtrasadas, int ObrasConcluidas,
    decimal ValorTotalPortfolio, decimal ReceitaMes, decimal DespesaMes,
    decimal SaldoMes, int TotalFuncionarios, int TotalAcidentesMes);
