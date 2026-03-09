namespace Constriva.Application.Features.Compras.DTOs;

public record ItemPedidoDto(Guid Id, string Descricao, string UnidadeMedida,
    decimal Quantidade, decimal PrecoUnitario, decimal ValorTotal);
