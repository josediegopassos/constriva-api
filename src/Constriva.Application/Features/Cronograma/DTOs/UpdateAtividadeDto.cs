namespace Constriva.Application.Features.Cronograma.DTOs;

public record UpdateAtividadeDto(string Nome, string? Descricao, DateTime DataInicio, DateTime DataFim, decimal PercentualConcluido, string? Responsavel);
