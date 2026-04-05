using Constriva.Domain.Entities.Compras;

namespace Constriva.Application.Features.Compras.DTOs;

public static class CotacaoMapper
{
    public static CotacaoDto ToDto(Cotacao c) => new(
        c.Id, c.Numero, c.Titulo, c.ObraId, c.Obra?.Nome, c.Status,
        c.DataAbertura, c.DataFechamento, c.DataLimiteResposta,
        c.Observacoes, c.CondicoesGerais, c.FornecedorVencedorId,
        c.FornecedorVencedor?.NomeFantasia ?? c.FornecedorVencedor?.RazaoSocial,
        c.FornecedoresConvidados?.Count(f => !f.IsDeleted) ?? 0,
        c.Propostas?.Count(p => !p.IsDeleted) ?? 0,
        c.CreatedAt,
        (c.Itens ?? new List<ItemCotacao>())
            .Where(i => !i.IsDeleted)
            .OrderBy(i => i.Ordem)
            .Select(i => new ItemCotacaoDto(i.Id, i.MaterialId,
                i.Descricao, i.UnidadeMedida, i.Quantidade,
                i.Especificacao, i.PrecoReferencia, i.Ordem)));
}
