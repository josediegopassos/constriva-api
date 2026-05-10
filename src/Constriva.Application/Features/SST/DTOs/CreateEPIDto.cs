using Constriva.Domain.Enums;

namespace Constriva.Application.Features.SST.DTOs;

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
