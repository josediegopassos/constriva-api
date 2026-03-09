using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record DeleteFuncionarioCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteFuncionarioCommandHandler : IRequestHandler<DeleteFuncionarioCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteFuncionarioCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var funcionario = await _repo.GetFuncionarioByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Funcionário {request.Id} não encontrado.");

        funcionario.Status = StatusFuncionarioEnum.Demitido;
        funcionario.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
