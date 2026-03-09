using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

public record DeleteDDSCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteDDSCommandHandler : IRequestHandler<DeleteDDSCommand, Unit>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteDDSCommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteDDSCommand request, CancellationToken cancellationToken)
    {
        var dds = await _repo.GetDDSByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"DDS {request.Id} não encontrado.");

        dds.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
