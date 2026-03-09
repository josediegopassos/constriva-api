using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

public record DeleteAcidenteCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteAcidenteCommandHandler : IRequestHandler<DeleteAcidenteCommand, Unit>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteAcidenteCommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteAcidenteCommand request, CancellationToken cancellationToken)
    {
        var acidente = await _repo.GetAcidenteByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Acidente {request.Id} não encontrado.");

        acidente.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
