using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record PedidoCompraDto(
    Guid Id,
    string Numero,
    Guid? ObraId,
    string? ObraNome,
    Guid? FornecedorId,
    string? FornecedorNome,
    Guid? CotacaoId,
    Guid? AlmoxarifadoId,
    StatusPedidoCompraEnum Status,
    DateTime DataPedido,
    DateTime? DataEntregaPrevista,
    DateTime? DataEntregaReal,
    FormaPagamentoEnum? FormaPagamento,
    string? CondicoesPagamento,
    string? LocalEntrega,
    decimal ValorFrete,
    decimal ValorDesconto,
    decimal ValorTotal,
    string? Observacoes,
    string? MotivoRejeicao,
    Guid? AprovadoPor,
    DateTime? DataAprovacao,
    DateTime CreatedAt,
    IEnumerable<ItemPedidoDto> Itens);
