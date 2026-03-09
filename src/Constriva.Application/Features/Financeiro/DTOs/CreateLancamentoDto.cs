using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Financeiro.DTOs;

public record CreateLancamentoDto(
    TipoLancamentoEnum Tipo, string Descricao, decimal Valor, DateTime DataVencimento,
    Guid? ObraId, Guid? FornecedorId, Guid? CentroCustoId, Guid? ContaBancariaId,
    FormaPagamentoEnum? FormaPagamentoEnum, string? NumeroDocumento, string? NumeroNF,
    string? Observacoes, bool Repetir = false, string? Periodicidade = null, int? QtdParcelas = null
);
