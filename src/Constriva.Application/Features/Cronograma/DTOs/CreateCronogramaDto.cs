namespace Constriva.Application.Features.Cronograma.DTOs;

public record CreateCronogramaDto(
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string? Descricao = null,
    string? Observacoes = null);
