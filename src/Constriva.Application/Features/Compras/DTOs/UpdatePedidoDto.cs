using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record UpdatePedidoDto(
    Guid? ObraId = null,
    Guid? FornecedorId = null,
    Guid? AlmoxarifadoId = null,
    DateTime? DataEntregaPrevista = null,
    FormaPagamentoEnum? FormaPagamento = null,
    string? CondicoesPagamento = null,
    string? LocalEntrega = null,
    decimal? ValorFrete = null,
    decimal? ValorDesconto = null,
    string? Observacoes = null,
    IEnumerable<CreateItemPedidoDto>? Itens = null);
