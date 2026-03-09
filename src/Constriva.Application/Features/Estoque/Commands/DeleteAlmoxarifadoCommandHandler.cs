using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record DeleteAlmoxarifadoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteAlmoxarifadoHandler : IRequestHandler<DeleteAlmoxarifadoCommand, Unit>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteAlmoxarifadoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteAlmoxarifadoCommand request, CancellationToken cancellationToken)
    {
        var almoxarifado = await _repo.GetAlmoxarifadoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Almoxarifado {request.Id} não encontrado.");

        almoxarifado.Ativo = false;
        almoxarifado.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
