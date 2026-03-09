using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record UpdatePercentualObraCommand(Guid Id, Guid EmpresaId, decimal Percentual)
    : IRequest<Unit>, ITenantRequest;

public class UpdatePercentualObraCommandHandler : IRequestHandler<UpdatePercentualObraCommand, Unit>
{
    private readonly IObraRepository _repo;
    private readonly IUnitOfWork _uow;
    public UpdatePercentualObraCommandHandler(IObraRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(UpdatePercentualObraCommand r, CancellationToken ct)
    {
        var obra = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {r.Id} não encontrada.");
        obra.PercentualConcluido = r.Percentual;
        if (r.Percentual >= 100 && obra.DataFimReal == null)
            obra.DataFimReal = DateTime.Today;
        obra.UpdatedAt = DateTime.UtcNow;
        _repo.Update(obra);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

