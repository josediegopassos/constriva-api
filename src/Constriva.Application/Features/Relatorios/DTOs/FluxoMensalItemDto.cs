namespace Constriva.Application.Features.Relatorios.DTOs;

public record FluxoMensalItemDto(int Mes, int Ano, decimal Receitas, decimal Despesas, decimal Saldo);
