using Constriva.Application.Features.Agente.DTOs;

namespace Constriva.Application.Features.Agente.Services;

public interface IAgentService
{
    Task<ChatResponseDto> ChatAsync(Guid empresaId, Guid usuarioId, ChatRequestDto request, CancellationToken ct);
}
