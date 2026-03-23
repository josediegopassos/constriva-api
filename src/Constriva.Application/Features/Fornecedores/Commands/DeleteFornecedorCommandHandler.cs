using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Fornecedores.Commands;

public record DeleteFornecedorCommand(Guid Id, Guid EmpresaId, Guid UsuarioId)
    : IRequest<Unit>, ITenantRequest;

public class DeleteFornecedorCommandHandler : IRequestHandler<DeleteFornecedorCommand, Unit>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteFornecedorCommandHandler(IFornecedorRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DeleteFornecedorCommand r, CancellationToken ct)
    {
        var fornecedor = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Fornecedor {r.Id} não encontrado.");

        fornecedor.Ativo = false;
        fornecedor.IsDeleted = true;
        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
