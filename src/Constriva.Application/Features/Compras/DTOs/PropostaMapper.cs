using Constriva.Domain.Entities.Compras;

namespace Constriva.Application.Features.Compras.DTOs;

public static class PropostaMapper
{
    public static PropostaDto ToDto(PropostaCotacao p) => new(
        p.Id, p.CotacaoId, p.FornecedorId,
        p.Fornecedor?.NomeFantasia ?? p.Fornecedor?.RazaoSocial ?? "",
        p.DataRecebimento, p.DataValidade,
        p.CondicoesPagamento, p.PrazoEntrega,
        p.Observacoes, p.ValorTotal, p.Vencedora,
        (p.Itens ?? new List<ItemPropostaCotacao>())
            .Where(i => !i.IsDeleted)
            .Select(i => new ItemPropostaDto(
                i.Id, i.ItemCotacaoId,
                i.ItemCotacao?.Descricao ?? "",
                i.PrecoUnitario, i.Quantidade, i.ValorTotal,
                i.Marca, i.Observacao, i.Disponivel, i.MenorPreco)));
}
