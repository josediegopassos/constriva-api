using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Services;

namespace Constriva.Application.Features.Agente.Queries;

public record GetDashboardConsumoQuery(Guid EmpresaId)
    : IRequest<DashboardConsumoDto>, ITenantRequest;

public class GetDashboardConsumoHandler : IRequestHandler<GetDashboardConsumoQuery, DashboardConsumoDto>
{
    private readonly IAgenteTokenService _tokenService;
    public GetDashboardConsumoHandler(IAgenteTokenService tokenService) => _tokenService = tokenService;

    public async Task<DashboardConsumoDto> Handle(GetDashboardConsumoQuery r, CancellationToken ct)
    {
        return await _tokenService.ObterDashboardConsumoAsync(r.EmpresaId, ct);
    }
}
