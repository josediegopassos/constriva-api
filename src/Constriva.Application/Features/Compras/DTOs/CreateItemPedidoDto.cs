namespace Constriva.Application.Features.Compras.DTOs;

public record CreateItemPedidoDto(string Descricao, string UnidadeMedida,
    decimal Quantidade, decimal PrecoUnitario, Guid? MaterialId = null);
