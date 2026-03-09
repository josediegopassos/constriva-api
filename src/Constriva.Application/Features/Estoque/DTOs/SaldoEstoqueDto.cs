namespace Constriva.Application.Features.Estoque.DTOs;

public record SaldoEstoqueDto(
    Guid Id, Guid AlmoxarifadoId, string AlmoxarifadoNome,
    Guid MaterialId, string MaterialNome, string MaterialCodigo, string Unidade,
    decimal SaldoAtual, decimal SaldoReservado, decimal SaldoDisponivel,
    decimal CustoMedio, DateTime? UltimaMovimentacao);
