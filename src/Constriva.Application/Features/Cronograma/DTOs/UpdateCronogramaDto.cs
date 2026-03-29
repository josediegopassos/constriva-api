namespace Constriva.Application.Features.Cronograma.DTOs;

public record UpdateCronogramaDto(
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string? Descricao = null,
    string? Observacoes = null,
    bool ELinhaDBase = false,
    bool Ativo = true);
