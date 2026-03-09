// ─── CRONOGRAMA ───────────────────────────────────────────────────────────────
using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record DeleteAtividadeCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteAtividadeCommandHandler : IRequestHandler<DeleteAtividadeCommand, Unit>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteAtividadeCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteAtividadeCommand request, CancellationToken cancellationToken)
    {
        var atividade = await _repo.GetAtividadeByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Atividade não encontrada.");

        atividade.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
