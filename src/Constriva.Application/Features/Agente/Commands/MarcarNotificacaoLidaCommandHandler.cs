using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Commands;

public record MarcarNotificacaoLidaCommand(Guid Id, Guid EmpresaId)
    : IRequest<Unit>, ITenantRequest;

public class MarcarNotificacaoLidaCommandHandler : IRequestHandler<MarcarNotificacaoLidaCommand, Unit>
{
    private readonly IAgenteRepository _repo;
    private readonly IUnitOfWork _uow;
    public MarcarNotificacaoLidaCommandHandler(IAgenteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(MarcarNotificacaoLidaCommand r, CancellationToken ct)
    {
        var notificacao = await _repo.GetNotificacaoByIdAsync(r.Id, r.EmpresaId, ct);
        if (notificacao == null) throw new KeyNotFoundException("Notificação não encontrada.");

        notificacao.Lida = true;
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
