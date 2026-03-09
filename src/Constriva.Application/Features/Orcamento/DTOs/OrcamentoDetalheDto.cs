using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record OrcamentoDetalheDto(
    Guid Id,
    string Codigo,
    string Nome,
    StatusOrcamentoEnum Status,
    string StatusLabel,
    int Versao,
    bool ELinhaDBase,
    decimal BDI,
    decimal BDIDetalhadoAdministracao,
    decimal BDIDetalhadoSeguro,
    decimal BDIDetalhadoRisco,
    decimal BDIDetalhadoFinanceiro,
    decimal BDIDetalhadoLucro,
    decimal BDIDetalhadoTributos,
    decimal EncargosHoristas,
    decimal EncargosMensalistas,
    decimal ValorCustoDirecto,
    decimal ValorBDI,
    decimal ValorTotal,
    DateTime DataReferencia,
    string? BaseOrcamentaria,
    string? Localidade,
    string? Descricao,
    Guid? AprovadoPor,
    DateTime? DataAprovacao,
    string? Observacoes,
    IEnumerable<GrupoOrcamentoDto> Grupos);
