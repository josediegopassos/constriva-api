namespace Constriva.Application.Features.Lens.DTOs;

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
