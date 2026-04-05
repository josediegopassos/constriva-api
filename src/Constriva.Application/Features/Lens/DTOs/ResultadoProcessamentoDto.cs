using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.DTOs;

public record ResultadoProcessamentoDto(
    Guid Id,
    TipoDocumentoLensEnum TipoDocumento,
    TipoDocumentoLensEnum TipoDocumentoDeclarado,
    bool TiposConferem,
    StatusProcessamentoLensEnum Status,
    float? ConfidenceScore,
    List<string> Warnings,
    int? TempoProcessamentoMs,
    int? PaginasProcessadas,
    List<ItemDocumentoLensDto> Itens,
    SugestaoMatchingDto? SugestaoMatching,
    DadosResumidosDto? DadosResumidos,
    MetodoExtracaoLensEnum MetodoExtracao,
    string MetodoExtracaoDescricao,
    bool PodeReprocessar,
    bool PodeConsolidar);

public record DadosResumidosDto(
    string? Numero,
    string? DataEmissao,
    decimal? ValorTotal,
    string? Emitente,
    string? Destinatario);
