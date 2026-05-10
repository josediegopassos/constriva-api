namespace Constriva.Application.Features.Lens.DTOs;

public record DadosResumidosDto(
    string? Numero,
    string? DataEmissao,
    decimal? ValorTotal,
    string? Emitente,
    string? Destinatario);
