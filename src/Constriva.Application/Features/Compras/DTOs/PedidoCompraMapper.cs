using Constriva.Domain.Entities.Compras;

namespace Constriva.Application.Features.Compras.DTOs;

public static class PedidoCompraMapper
{
    public static PedidoCompraDto ToDto(PedidoCompra p, string? obraNome = null, string? fornecedorNome = null) => new(
        p.Id, p.Numero,
        p.ObraId == Guid.Empty ? null : p.ObraId, obraNome ?? p.Obra?.Nome,
        p.FornecedorId, fornecedorNome ?? p.Fornecedor?.NomeFantasia ?? p.Fornecedor?.RazaoSocial,
        p.CotacaoId, p.AlmoxarifadoId,
        p.Status, p.DataPedido, p.DataEntregaPrevista, p.DataEntregaReal,
        p.FormaPagamento, p.CondicoesPagamento, p.LocalEntrega,
        p.ValorFrete, p.ValorDesconto, p.ValorTotal,
        p.Observacoes, p.MotivoRejeicao, p.AprovadoPor, p.DataAprovacao,
        p.CreatedAt,
        (p.Itens ?? new List<ItemPedidoCompra>())
            .Where(i => !i.IsDeleted)
            .Select(i => new ItemPedidoDto(i.Id, i.PedidoId, i.MaterialId,
                i.Descricao, i.UnidadeMedida, i.QuantidadePedida, i.QuantidadeRecebida,
                i.QuantidadePendente, i.PrecoUnitario, i.ValorTotal)));
}
