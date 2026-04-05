using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.DTOs;

public record ProcessamentoLensDto(
    Guid Id,
    TipoDocumentoLensEnum TipoDocumento,
    string TipoDocumentoDescricao,
    StatusProcessamentoLensEnum Status,
    string StatusDescricao,
    string NomeArquivo,
    string ExtensaoArquivo,
    float? ConfidenceScore,
    int TotalItens,
    int ItensAprovados,
    int ItensRejeitados,
    int ItensPendentes,
    string? NomeFornecedorExtraido,
    decimal? ValorTotalExtraido,
    string? DataEmissaoExtraida,
    string? Observacoes,
    MetodoExtracaoLensEnum MetodoExtracao,
    string MetodoExtracaoDescricao,
    DateTime CriadoEm,
    DateTime? AtualizadoEm,
    string NomeUsuario);
