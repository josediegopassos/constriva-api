using Constriva.Domain.Enums;

namespace Constriva.Application.Features.SST.DTOs;

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
