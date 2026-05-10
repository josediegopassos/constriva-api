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
