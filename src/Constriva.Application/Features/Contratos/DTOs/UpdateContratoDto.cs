using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record UpdateContratoDto(
    string Objeto,
    TipoContratoFornecedorEnum Tipo,
    Guid FornecedorId,
    decimal ValorGlobal,
    DateTime DataAssinatura,
    DateTime DataVigenciaInicio,
    DateTime DataVigenciaFim,
    decimal PercentualRetencao,
    string? Descricao = null,
    string? CondicoesPagamento = null,
    int? DiasParaMedicao = null,
    int? DiasParaPagamento = null,
    string? ArquivoUrl = null,
    Guid? AssinadoPor = null,
    Guid? FiscalId = null,
    string? Observacoes = null);
