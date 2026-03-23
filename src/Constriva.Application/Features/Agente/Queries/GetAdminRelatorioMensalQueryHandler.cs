using MediatR;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Application.Features.Agente.Services;

namespace Constriva.Application.Features.Agente.Queries;

public record GetAdminRelatorioMensalQuery(int Ano, int Mes)
    : IRequest<IEnumerable<AdminRelatorioItemDto>>;

public class GetAdminRelatorioMensalHandler : IRequestHandler<GetAdminRelatorioMensalQuery, IEnumerable<AdminRelatorioItemDto>>
{
    private readonly IAgenteTokenService _tokenService;
    public GetAdminRelatorioMensalHandler(IAgenteTokenService tokenService) => _tokenService = tokenService;

    public async Task<IEnumerable<AdminRelatorioItemDto>> Handle(GetAdminRelatorioMensalQuery r, CancellationToken ct)
    {
        return await _tokenService.ObterRelatorioMensalAsync(r.Ano, r.Mes, ct);
    }
}
