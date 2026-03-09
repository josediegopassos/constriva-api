namespace Constriva.Application.Features.Compras.DTOs;

public record CreatePedidoDto(
    Guid? ObraId, Guid? FornecedorId,
    string? Observacoes, IEnumerable<CreateItemPedidoDto> Itens);
