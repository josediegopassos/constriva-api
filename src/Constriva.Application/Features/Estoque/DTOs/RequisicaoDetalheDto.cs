using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record RequisicaoDetalheDto(
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
    DateTime DataRequisicao,
    DateTime? DataNecessidade,
    DateTime? DataAprovacao,
    string? MotivoRejeicao,
    string? Observacoes,
    DateTime CreatedAt,
    IEnumerable<ItemRequisicaoDto> Itens);
