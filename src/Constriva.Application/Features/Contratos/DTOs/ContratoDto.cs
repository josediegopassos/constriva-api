using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record ContratoDto(
    Guid Id, string Numero, string Objeto, TipoContratoFornecedorEnum Tipo, StatusContratoEnum Status,
    Guid ObraId, Guid FornecedorId, string? NomeFornecedor,
    decimal ValorGlobal, decimal ValorMedidoAcumulado, decimal PercentualMedido,
    DateTime DataAssinatura, DateTime DataVigenciaInicio, DateTime DataVigenciaFim,
    string? Observacoes, DateTime CreatedAt);

public record ContratoDetalheDto(
    Guid Id, string Numero, string Objeto, string? Descricao,
    TipoContratoFornecedorEnum Tipo, StatusContratoEnum Status,
    Guid ObraId, Guid FornecedorId, string? NomeFornecedor,
    decimal ValorGlobal, decimal ValorAditivos, decimal ValorTotal,
    decimal ValorMedidoAcumulado, decimal ValorPagoAcumulado,
    decimal PercentualRetencao, decimal ValorRetencao,
    string? CondicoesPagamento, int? DiasParaMedicao, int? DiasParaPagamento,
    DateTime DataAssinatura, DateTime DataVigenciaInicio, DateTime DataVigenciaFim,
    DateTime? DataEncerramento,
    string? ArquivoUrl, Guid? AssinadoPor, Guid? FiscalId,
    string? Observacoes, DateTime CreatedAt);
