using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record MovimentacaoEstoqueDto(
    Guid Id,
    TipoMovimentacaoEstoqueEnum Tipo,
    string TipoLabel,
    Guid AlmoxarifadoId,
    string AlmoxarifadoNome,
    Guid MaterialId,
    string MaterialNome,
    string MaterialCodigo,
    decimal Quantidade,
    decimal PrecoUnitario,
    decimal ValorTotal,
    decimal SaldoAnterior,
    decimal SaldoPosterior,
    DateTime DataMovimentacao,
    string? NumeroDocumento,
    string? NumeroNF,
    string? Lote,
    Guid? ObraId,
    Guid UsuarioId,
    string UsuarioNome,
    DateTime CreatedAt);
