namespace Constriva.Application.Features.Cronograma.DTOs;

public record CreateAtividadeDto(
    string Nome, string? Descricao, int Ordem,
    DateTime DataInicioPrevista, DateTime DataFimPrevista,
    decimal DuracaoDias, IEnumerable<Guid>? Predecessoras = null
);
