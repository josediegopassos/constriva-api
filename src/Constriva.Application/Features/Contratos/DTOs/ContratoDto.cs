using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record ContratoDto(
    Guid Id, string Numero, string Objeto, TipoContratoFornecedorEnum Tipo, StatusContratoEnum Status,
    Guid ObraId, Guid FornecedorId, string? NomeFornecedor,
    decimal ValorGlobal, decimal ValorMedidoAcumulado, decimal PercentualMedido,
    DateTime DataAssinatura, DateTime DataVigenciaInicio, DateTime DataVigenciaFim,
    string? Observacoes, DateTime CreatedAt);
