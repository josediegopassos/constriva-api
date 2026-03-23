using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Commands;

public record DeleteSessaoCommand(Guid SessaoId, Guid EmpresaId)
    : IRequest<Unit>, ITenantRequest;

public class DeleteSessaoCommandHandler : IRequestHandler<DeleteSessaoCommand, Unit>
{
    private readonly IAgenteRepository _repo;
    private readonly IUnitOfWork _uow;
    public DeleteSessaoCommandHandler(IAgenteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DeleteSessaoCommand r, CancellationToken ct)
    {
        var sessao = await _repo.GetSessaoByIdAsync(r.SessaoId, r.EmpresaId, ct);
        if (sessao == null) throw new KeyNotFoundException("Sessão não encontrada.");

        sessao.Ativa = false;
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
