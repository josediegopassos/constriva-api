namespace Constriva.Application.Features.GED.DTOs;

public record PastaDto(
    Guid Id, string Nome, string? Descricao, Guid? ObraId, Guid? PastaPaiId,
    bool AcessoPublico, int TotalDocumentos);
