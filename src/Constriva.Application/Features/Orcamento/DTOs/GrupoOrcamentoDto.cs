namespace Constriva.Application.Features.Orcamento.DTOs;

public record GrupoOrcamentoDto(
    Guid Id,
    string Codigo,
    string Nome,
    int Ordem,
    decimal ValorTotal,
    decimal PercentualTotal,
    Guid? GrupoPaiId,
    IEnumerable<GrupoOrcamentoDto> SubGrupos,
    IEnumerable<ItemOrcamentoDto> Itens);
