namespace Constriva.Application.Features.GED.DTOs;

public record UpdateDocumentoDto(string Nome, string? Descricao, DateTime? DataVencimento, string? Tags);
