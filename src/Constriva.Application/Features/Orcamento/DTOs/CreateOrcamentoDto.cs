namespace Constriva.Application.Features.Orcamento.DTOs;

public record CreateOrcamentoDto(
    string Nome,
    decimal BDI,
    DateTime DataReferencia,
    string? Descricao = null,
    string? Observacoes = null,
    string? BaseOrcamentaria = null,
    string? Localidade = null,
    decimal BDIDetalhadoAdministracao = 0,
    decimal BDIDetalhadoSeguro = 0,
    decimal BDIDetalhadoRisco = 0,
    decimal BDIDetalhadoFinanceiro = 0,
    decimal BDIDetalhadoLucro = 0,
    decimal BDIDetalhadoTributos = 0,
    decimal EncargosHoristas = 0,
    decimal EncargosMensalistas = 0);
