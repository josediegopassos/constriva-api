namespace Constriva.Application.Features.Estoque.DTOs;

public record UpdateRequisicaoDto(
    string Motivo,
    DateTime? DataNecessidade,
    string? Observacoes);
