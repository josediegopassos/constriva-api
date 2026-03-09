using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade.Commands;

public record DeleteInspecaoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteInspecaoCommandHandler : IRequestHandler<DeleteInspecaoCommand, Unit>
{
    private readonly IQualidadeRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteInspecaoCommandHandler(IQualidadeRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteInspecaoCommand request, CancellationToken cancellationToken)
    {
        var inspecao = await _repo.GetInspecaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Inspeção {request.Id} não encontrada.");

        inspecao.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
