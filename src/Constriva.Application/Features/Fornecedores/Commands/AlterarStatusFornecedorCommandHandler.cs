using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Fornecedores.Commands;

public record AtivarFornecedorCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest;
public record DesativarFornecedorCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest;

public class AtivarFornecedorHandler : IRequestHandler<AtivarFornecedorCommand, Unit>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public AtivarFornecedorHandler(IFornecedorRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AtivarFornecedorCommand r, CancellationToken ct)
    {
        var fornecedor = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Fornecedor {r.Id} não encontrado.");

        fornecedor.Ativo = true;
        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class DesativarFornecedorHandler : IRequestHandler<DesativarFornecedorCommand, Unit>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public DesativarFornecedorHandler(IFornecedorRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DesativarFornecedorCommand r, CancellationToken ct)
    {
        var fornecedor = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Fornecedor {r.Id} não encontrado.");

        fornecedor.Ativo = false;
        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public record HomologarFornecedorCommand(Guid Id, Guid EmpresaId, bool Homologado) : IRequest<Unit>, ITenantRequest;

public class HomologarFornecedorHandler : IRequestHandler<HomologarFornecedorCommand, Unit>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public HomologarFornecedorHandler(IFornecedorRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(HomologarFornecedorCommand r, CancellationToken ct)
    {
        var fornecedor = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Fornecedor {r.Id} não encontrado.");

        fornecedor.Homologado = r.Homologado;
        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
