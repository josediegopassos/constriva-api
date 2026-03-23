using MediatR;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Services;

namespace Constriva.Application.Features.Agente.Commands;

public record CriarCotaAvulsaCommand(Guid EmpresaId, Guid AdminUsuarioId, CriarCotaAvulsaDto Dto)
    : IRequest<Unit>;

public class CriarCotaAvulsaCommandHandler : IRequestHandler<CriarCotaAvulsaCommand, Unit>
{
    private readonly IAgenteTokenService _tokenService;
    public CriarCotaAvulsaCommandHandler(IAgenteTokenService tokenService) => _tokenService = tokenService;

    public async Task<Unit> Handle(CriarCotaAvulsaCommand r, CancellationToken ct)
    {
        await _tokenService.AdicionarCotaAvulsaAsync(
            r.Dto.EmpresaId, r.Dto.Tokens, r.Dto.Motivo, r.Dto.Expiracao, r.AdminUsuarioId, ct);

        return Unit.Value;
    }
}
