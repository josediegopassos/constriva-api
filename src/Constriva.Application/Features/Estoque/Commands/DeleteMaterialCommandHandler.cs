using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record DeleteMaterialCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteMaterialHandler : IRequestHandler<DeleteMaterialCommand, Unit>
{
    private readonly IMaterialRepository _repo;
    private readonly IEstoqueRepository _estoqueRepo;
    private readonly IUnitOfWork _uow;

    public DeleteMaterialHandler(IMaterialRepository repo, IEstoqueRepository estoqueRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _estoqueRepo = estoqueRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Material {request.Id} não encontrado.");

        await _estoqueRepo.SoftDeleteByMaterialIdAsync(request.Id, request.EmpresaId, cancellationToken);

        material.Ativo = false;
        material.IsDeleted = true;
        _repo.Update(material);

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
