using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Financeiro.DTOs;

public record UpdateLancamentoDto(
    string? Descricao, decimal? Valor, DateTime? DataVencimento,
    Guid? ObraId, Guid? FornecedorId, Guid? CentroCustoId,
    FormaPagamentoEnum? FormaPagamentoEnum, string? NumeroDocumento,
    string? NumeroNF, string? Observacoes
);
