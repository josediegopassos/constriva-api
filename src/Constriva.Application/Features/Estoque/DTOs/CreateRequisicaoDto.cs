using Constriva.Application.Features.Estoque.Commands;

namespace Constriva.Application.Features.Estoque.DTOs;

public record CreateRequisicaoDto(
    Guid ObraId,
    Guid AlmoxarifadoId,
    string Motivo,
    Guid? FaseObraId,
    DateTime? DataNecessidade,
    string? Observacoes,
    IEnumerable<AddItemRequisicaoDto>? Itens = null);
