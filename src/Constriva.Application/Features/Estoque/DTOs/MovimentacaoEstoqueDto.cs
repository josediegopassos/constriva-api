using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record MovimentacaoEstoqueDto(
    Guid Id, TipoMovimentacaoEstoqueEnum Tipo, string TipoLabel,
    string MaterialNome, string MaterialCodigo,
    decimal Quantidade, decimal PrecoUnitario, decimal ValorTotal,
    decimal SaldoAnterior, decimal SaldoPosterior,
    Guid? ObraId, DateTime CreatedAt);
