using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Cronograma.DTOs;

public record AtividadeDto(
    Guid Id, string Nome, string? Descricao, int Ordem,
    DateTime DataInicioPrevista, DateTime DataFimPrevista,
    DateTime? DataInicioReal, DateTime? DataFimReal,
    decimal PercentualConcluido, StatusAtividadeEnum Status, bool IsCaminhoCritico,
    IEnumerable<Guid> Predecessoras
);
