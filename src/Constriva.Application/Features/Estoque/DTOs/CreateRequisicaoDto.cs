namespace Constriva.Application.Features.Estoque.DTOs;

public record CreateRequisicaoDto(Guid ObraId, Guid AlmoxarifadoId, string Motivo);
