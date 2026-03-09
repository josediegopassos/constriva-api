namespace Constriva.Application.Features.Estoque.DTOs;

public record UpdateRequisicaoDto(string Descricao, DateTime DataNecessidade, string? Observacoes);
