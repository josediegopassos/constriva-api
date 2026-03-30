// ─── CRONOGRAMA ───────────────────────────────────────────────────────────────
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Cronograma.Commands;

public record DeleteCronogramaCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteCronogramaCommandHandler : IRequestHandler<DeleteCronogramaCommand, Unit>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteCronogramaCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteCronogramaCommand request, CancellationToken cancellationToken)
    {
        var cronograma = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Cronograma não encontrado.");

        cronograma.IsDeleted = true;
        cronograma.DeletedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
