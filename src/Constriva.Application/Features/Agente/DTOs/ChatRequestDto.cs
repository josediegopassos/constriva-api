namespace Constriva.Application.Features.Agente.DTOs;

public record ChatRequestDto(string Mensagem, Guid? SessaoId = null);
