using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Queries;

public record GetNotificacoesQuery(Guid EmpresaId, bool? Lida = null)
    : IRequest<IEnumerable<NotificacaoDto>>, ITenantRequest;

public class GetNotificacoesHandler : IRequestHandler<GetNotificacoesQuery, IEnumerable<NotificacaoDto>>
{
    private readonly IAgenteRepository _repo;
    public GetNotificacoesHandler(IAgenteRepository repo) => _repo = repo;

    public async Task<IEnumerable<NotificacaoDto>> Handle(GetNotificacoesQuery r, CancellationToken ct)
    {
        var items = await _repo.GetNotificacoesAsync(r.EmpresaId, r.Lida, ct);

        return items.Select(n => new NotificacaoDto(
            n.Id, n.ModuloOrigem, n.Tipo.ToString(), n.Mensagem,
            n.Lida, n.Prazo, n.CreatedAt));
    }
}
