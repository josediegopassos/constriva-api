using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.RH.Commands;

public record AlterarStatusFuncionarioCommand(Guid Id, Guid EmpresaId, StatusFuncionarioEnum Status, string? Motivo)
    : IRequest<Unit>, ITenantRequest;

public class AlterarStatusFuncionarioHandler : IRequestHandler<AlterarStatusFuncionarioCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public AlterarStatusFuncionarioHandler(IRHRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AlterarStatusFuncionarioCommand r, CancellationToken ct)
    {
        var f = await _repo.GetFuncionarioByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Funcionário {r.Id} não encontrado.");

        if (f.Status == r.Status)
            throw new InvalidOperationException($"Funcionário já está com status {r.Status}.");

        f.Status = r.Status;

        if (r.Status == StatusFuncionarioEnum.Demitido)
        {
            f.DataDemissao = DateTime.UtcNow;
            if (r.Motivo != null) f.MotivoDemissao = r.Motivo;
        }

        f.UpdatedAt = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
