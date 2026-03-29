namespace Constriva.Application.Features.Cronograma.DTOs;

public record CronogramaDetalhadoDto(
    Guid Id, Guid ObraId, string Nome, string ObraNome,
    string? Descricao, string? Observacoes,
    bool ELinhaDBase, int Versao, Guid? VersaoBaseadaEm,
    DateTime DataInicio, DateTime DataFim,
    decimal PercentualPrevisto, decimal PercentualRealizado,
    IEnumerable<AtividadeDetalhadaDto> Atividades
);
