using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record PedidoCompraDto(
    Guid Id, string Numero, Guid? ObraId, string? ObraNome,
    Guid? FornecedorId, string? FornecedorNome, StatusPedidoCompraEnum Status,
    decimal ValorTotal, DateTime DataPedido, string? Observacoes,
    IEnumerable<ItemPedidoDto> Itens
);
