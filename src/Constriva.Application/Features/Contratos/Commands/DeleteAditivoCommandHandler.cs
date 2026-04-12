using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Contratos.Commands;

public record DeleteAditivoCommand(Guid Id, Guid ContratoId, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteAditivoHandler : IRequestHandler<DeleteAditivoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteAditivoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteAditivoCommand request, CancellationToken cancellationToken)
    {
        var aditivo = await _repo.GetAditivoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Aditivo {request.Id} não encontrado.");

        var contrato = await _repo.GetByIdAndEmpresaAsync(request.ContratoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Contrato não encontrado.");

        contrato.ValorAditivos -= aditivo.ValorAditivo;
        aditivo.IsDeleted = true;

        _repo.Update(contrato);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
