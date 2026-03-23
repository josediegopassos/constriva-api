using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Services;

namespace Constriva.Application.Features.Agente.Commands;

public record ChatCommand(Guid EmpresaId, Guid UsuarioId, ChatRequestDto Dto)
    : IRequest<ChatResponseDto>, ITenantRequest;

public class ChatCommandHandler : IRequestHandler<ChatCommand, ChatResponseDto>
{
    private readonly IAgentService _agentService;
    public ChatCommandHandler(IAgentService agentService) => _agentService = agentService;

    public async Task<ChatResponseDto> Handle(ChatCommand r, CancellationToken ct)
    {
        return await _agentService.ChatAsync(r.EmpresaId, r.UsuarioId, r.Dto, ct);
    }
}
