using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record CreatePedidoDto(
    Guid? ObraId,
    Guid? FornecedorId,
    Guid? CotacaoId = null,
    Guid? AlmoxarifadoId = null,
    DateTime? DataEntregaPrevista = null,
    FormaPagamentoEnum? FormaPagamento = null,
    string? CondicoesPagamento = null,
    string? LocalEntrega = null,
    decimal ValorFrete = 0,
    decimal ValorDesconto = 0,
    string? Observacoes = null,
    IEnumerable<CreateItemPedidoDto>? Itens = null);
