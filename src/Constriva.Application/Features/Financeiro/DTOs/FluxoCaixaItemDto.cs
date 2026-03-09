namespace Constriva.Application.Features.Financeiro.DTOs;

public record FluxoCaixaItemDto(int Ano, int Mes, decimal Receitas, decimal Despesas, decimal Saldo);
