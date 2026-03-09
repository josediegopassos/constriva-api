using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record DeleteFornecedorCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteFornecedorHandler : IRequestHandler<DeleteFornecedorCommand, Unit>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteFornecedorHandler(IFornecedorRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteFornecedorCommand request, CancellationToken cancellationToken)
    {
        var fornecedor = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Fornecedor {request.Id} não encontrado.");

        fornecedor.Ativo = false;
        fornecedor.IsDeleted = true;
        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
