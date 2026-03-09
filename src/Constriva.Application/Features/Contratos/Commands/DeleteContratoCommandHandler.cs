using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record DeleteContratoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteContratoHandler : IRequestHandler<DeleteContratoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteContratoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteContratoCommand request, CancellationToken cancellationToken)
    {
        var contrato = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Contrato {request.Id} não encontrado.");

        contrato.IsDeleted = true;
        _repo.Update(contrato);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
