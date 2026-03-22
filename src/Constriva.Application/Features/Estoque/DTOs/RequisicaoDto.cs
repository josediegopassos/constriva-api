using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record ItemRequisicaoDto(
    Guid Id,
    Guid MaterialId,
    string MaterialCodigo,
    string MaterialNome,
    string MaterialUnidade,
    decimal QuantidadeSolicitada,
    decimal QuantidadeAtendida,
    decimal? PrecoReferencia,
    string? Observacao);

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
