namespace Constriva.Application.Features.Agente.DTOs;

public record CriarCotaAvulsaDto(Guid EmpresaId, long Tokens, string Motivo, DateTime? Expiracao = null);
