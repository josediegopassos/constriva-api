using Constriva.Domain.Enums;

namespace Constriva.Application.Features.SST.DTOs;

public record EPIDto(
    Guid Id,
    string Codigo,
    string Nome,
    string? Descricao,
    TipoEPIEnum Tipo,
    string? Fabricante,
    string? Modelo,
    string? NumeroCA,
    DateTime? ValidadeCA,
    int EstoqueAtual,
    int EstoqueMinimo,
    decimal VidaUtilMeses,
    bool Ativo);

public record CreateEPIDto(
    string Codigo,
    string Nome,
    TipoEPIEnum Tipo,
    string? Descricao = null,
    string? Fabricante = null,
    string? Modelo = null,
    string? NumeroCA = null,
    DateTime? ValidadeCA = null,
    int EstoqueAtual = 0,
    int EstoqueMinimo = 0,
    decimal VidaUtilMeses = 12);

public record UpdateEPIDto(
    string Nome,
    TipoEPIEnum Tipo,
    string? Descricao,
    string? Fabricante,
    string? Modelo,
    string? NumeroCA,
    DateTime? ValidadeCA,
    int EstoqueAtual,
    int EstoqueMinimo,
    decimal VidaUtilMeses,
    bool Ativo);
