namespace Constriva.Application.Features.Financeiro.DTOs;

public record DashboardFinanceiroDto(
    decimal TotalReceitas, decimal TotalDespesas, decimal Saldo,
    decimal ReceitasRealizadas, decimal DespesasRealizadas,
    int LancamentosVencidos, int LancamentosVencendo
);
