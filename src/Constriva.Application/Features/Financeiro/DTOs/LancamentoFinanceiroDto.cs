using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Financeiro.DTOs;

public record LancamentoFinanceiroDto(
    Guid Id, string Descricao, TipoLancamentoEnum Tipo, StatusLancamentoEnum Status,
    decimal Valor, decimal? ValorRealizado, DateTime DataVencimento,
    DateTime? DataPagamento, FormaPagamentoEnum? FormaPagamentoEnum,
    string? NumeroDocumento, string? NumeroNF, string? Observacoes,
    Guid? ObraId, string? ObraNome, Guid? FornecedorId, string? FornecedorNome,
    Guid? CentroCustoId, string? CentroCustoNome, DateTime CreatedAt
);
