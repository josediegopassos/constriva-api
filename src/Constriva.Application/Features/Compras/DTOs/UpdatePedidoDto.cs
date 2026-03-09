namespace Constriva.Application.Features.Compras.DTOs;

public record UpdatePedidoDto(Guid? FornecedorId, DateTime? DataEntregaPrevista, string? Observacoes);
