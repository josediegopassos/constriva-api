using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Clientes.Commands;

public record DeleteClienteCommand(Guid Id, Guid EmpresaId, Guid UsuarioId)
    : IRequest<Unit>, ITenantRequest;

public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, Unit>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteClienteCommandHandler(IClienteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DeleteClienteCommand r, CancellationToken ct)
    {
        var cliente = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cliente {r.Id} não encontrado.");

        cliente.SoftDelete(r.UsuarioId);
        _repo.Update(cliente);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
