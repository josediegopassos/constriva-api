namespace Constriva.Application.Features.Compras.DTOs;

public record ItemPedidoDto(
    Guid Id,
    Guid PedidoId,
    Guid? MaterialId,
    string Descricao,
    string UnidadeMedida,
    decimal QuantidadePedida,
    decimal QuantidadeRecebida,
    decimal QuantidadePendente,
    decimal PrecoUnitario,
    decimal ValorTotal);
