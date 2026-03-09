using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record DeletePontoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeletePontoCommandHandler : IRequestHandler<DeletePontoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeletePontoCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeletePontoCommand request, CancellationToken cancellationToken)
    {
        var ponto = await _repo.GetPontoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Registro de ponto {request.Id} não encontrado.");

        ponto.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
