using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Clientes.Commands;

public record AlterarStatusClienteCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, StatusClienteEnum Status)
    : IRequest<Unit>, ITenantRequest;

public class AlterarStatusClienteCommandHandler : IRequestHandler<AlterarStatusClienteCommand, Unit>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public AlterarStatusClienteCommandHandler(IClienteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AlterarStatusClienteCommand r, CancellationToken ct)
    {
        var cliente = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cliente {r.Id} não encontrado.");

        if (cliente.Status == r.Status)
            throw new InvalidOperationException($"Cliente já está com status {r.Status}.");

        cliente.Status = r.Status;
        cliente.UpdatedBy = r.UsuarioId;
        cliente.UpdatedAt = DateTime.UtcNow;

        _repo.Update(cliente);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
