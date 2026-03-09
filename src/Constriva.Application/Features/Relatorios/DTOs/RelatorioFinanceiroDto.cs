namespace Constriva.Application.Features.Relatorios.DTOs;

public record RelatorioFinanceiroDto(
    decimal TotalReceitas, decimal TotalDespesas, decimal Saldo,
    decimal ReceitasPagas, decimal DespesasPagas, decimal ReceitasEmAberto, decimal DespesasEmAberto,
    decimal DespesasVencidas, IEnumerable<FluxoMensalItemDto> FluxoMensal);
