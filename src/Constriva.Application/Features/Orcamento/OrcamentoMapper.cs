using Constriva.Application.Features.Orcamento.DTOs;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento;

public static class OrcamentoMapper
{
    public static OrcamentoResumoDto ToResumoDto(Domain.Entities.Orcamento.Orcamento o, int totalGrupos) =>
        new(o.Id, o.Codigo, o.Nome, o.Status, o.Status.ToString(),
            o.Versao, o.ELinhaDBase, o.BDI,
            o.ValorCustoDirecto, o.ValorBDI, o.ValorTotal,
            o.DataReferencia, o.BaseOrcamentaria, totalGrupos);

    public static OrcamentoDetalheDto ToDetalheDto(Domain.Entities.Orcamento.Orcamento o) =>
        new(o.Id, o.Codigo, o.Nome, o.Status, o.Status.ToString(),
            o.Versao, o.ELinhaDBase, o.BDI,
            o.BDIDetalhado_Administracao, o.BDIDetalhado_Seguro, o.BDIDetalhado_Risco,
            o.BDIDetalhado_Financeiro, o.BDIDetalhado_Lucro, o.BDIDetalhado_Tributos,
            o.EncargosHoristas, o.EncargosMensalistas,
            o.ValorCustoDirecto, o.ValorBDI, o.ValorTotal,
            o.DataReferencia, o.BaseOrcamentaria, o.Localidade, o.Descricao,
            o.AprovadoPor, o.DataAprovacao, o.Observacoes,
            (o.Grupos ?? []).Where(g => !g.IsDeleted && g.GrupoPaiId == null)
                            .OrderBy(g => g.Ordem)
                            .Select(ToGrupoDto));

    public static GrupoOrcamentoDto ToGrupoDto(GrupoOrcamento g) =>
        new(g.Id, g.Codigo, g.Nome, g.Ordem, g.ValorTotal, g.PercentualTotal,
            g.GrupoPaiId,
            (g.SubGrupos ?? []).Where(s => !s.IsDeleted).OrderBy(s => s.Ordem).Select(ToGrupoDto),
            (g.Itens ?? []).Where(i => !i.IsDeleted).OrderBy(i => i.Ordem).Select(ToItemDto));

    public static ItemOrcamentoDto ToItemDto(ItemOrcamento i) =>
        new(i.Id, i.Codigo, i.Descricao, i.Fonte, i.CodigoFonte,
            i.UnidadeMedida, i.Quantidade, i.CustoUnitario, i.CustoTotal,
            i.BDI, i.CustoComBDI, i.Ordem, i.ComposicaoId);

    public static ComposicaoOrcamentoDto ToComposicaoDto(ComposicaoOrcamento c) =>
        new(c.Id, c.Codigo, c.Descricao, c.UnidadeMedida, c.Fonte, c.CodigoFonte,
            c.CustoTotal, c.Observacoes,
            (c.Insumos ?? []).Select(i => new InsumoComposicaoDto(
                i.Id, i.Codigo, i.Descricao, i.Tipo,
                i.UnidadeMedida, i.Coeficiente, i.PrecoUnitario, i.CustoTotal,
                i.FontePrecoEnum)));
}
