using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record CreateContratoDto(
    Guid ObraId, Guid FornecedorId, string Objeto, TipoContratoFornecedorEnum Tipo,
    decimal ValorGlobal, DateTime DataAssinatura,
    DateTime DataVigenciaInicio, DateTime DataVigenciaFim,
    decimal PercentualRetencao = 0, string? Observacoes = null);
