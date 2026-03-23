using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Queries;

public record GetSessoesQuery(Guid EmpresaId, Guid UsuarioId)
    : IRequest<IEnumerable<SessaoResumoDto>>, ITenantRequest;

public class GetSessoesHandler : IRequestHandler<GetSessoesQuery, IEnumerable<SessaoResumoDto>>
{
    private readonly IAgenteRepository _repo;
    public GetSessoesHandler(IAgenteRepository repo) => _repo = repo;

    public async Task<IEnumerable<SessaoResumoDto>> Handle(GetSessoesQuery r, CancellationToken ct)
    {
        var sessoes = await _repo.GetSessoesByUsuarioAsync(r.EmpresaId, r.UsuarioId, ct);

        return sessoes.Select(s => new SessaoResumoDto(
            s.Id, s.AtualizadaEm, s.Ativa, s.Mensagens.Count));
    }
}
