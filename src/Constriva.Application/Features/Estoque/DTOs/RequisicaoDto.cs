using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record RequisicaoDto(
    Guid Id,
    string Numero,
    Guid ObraId,
    Guid? FaseObraId,
    Guid AlmoxarifadoId,
    string Motivo,
    StatusRequisicaoEnum Status,
    string StatusLabel,
    Guid SolicitanteId,
    string SolicitanteNome,
    Guid? AprovadorId,
    DateTime? DataNecessidade,
    DateTime? DataAprovacao,
    string? MotivoRejeicao,
    string? Observacoes,
    DateTime CreatedAt);
