using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record RequisicaoDto(
    Guid Id, string Numero, Guid ObraId, Guid AlmoxarifadoId,
    string Motivo, StatusRequisicaoEnum Status, string StatusLabel,
    Guid SolicitanteId, DateTime CreatedAt);
