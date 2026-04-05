using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.DTOs;

public record ProcessamentoUpdatedNotificationDto(
    Guid ProcessamentoId,
    StatusProcessamentoLensEnum Status,
    string StatusDescricao,
    string TipoDocumento,
    DateTime AtualizadoEm,
    string Mensagem);

public record ProcessamentoCompletedNotificationDto(
    Guid ProcessamentoId,
    string Status,
    string TipoDocumento,
    bool TiposConferem,
    float ConfidenceScore,
    int TotalItens,
    List<string> Warnings,
    string? FornecedorSugerido,
    string? CnpjFornecedor,
    decimal? ValorTotal,
    string? DataEmissao,
    int TempoProcessamentoMs,
    string Mensagem);

public record ProcessamentoErrorNotificationDto(
    Guid ProcessamentoId,
    string Status,
    string TipoDocumento,
    string CodigoErro,
    string MensagemErro,
    bool PodeReprocessar,
    string Mensagem);

public record ItemUpdatedNotificationDto(
    Guid ProcessamentoId,
    Guid ItemId,
    string Acao,
    Guid UsuarioId,
    string NomeUsuario,
    DateTime AtualizadoEm);

public record ConsolidationCompletedNotificationDto(
    Guid ProcessamentoId,
    Guid CompraId,
    int TotalItensConsolidados,
    int TotalItensRejeitados,
    decimal ValorTotal,
    string Mensagem);
