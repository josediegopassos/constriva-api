using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record OrcamentoResumoDto(
    Guid Id,
    string Codigo,
    string Nome,
    StatusOrcamentoEnum Status,
    string StatusLabel,
    int Versao,
    bool ELinhaDBase,
    decimal BDI,
    decimal ValorCustoDirecto,
    decimal ValorBDI,
    decimal ValorTotal,
    DateTime DataReferencia,
    string? BaseOrcamentaria,
    int TotalGrupos);
