using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record DeleteObraCommand(Guid Id, Guid EmpresaId, Guid UsuarioId)
    : IRequest<Unit>, ITenantRequest;

public class DeleteObraCommandHandler : IRequestHandler<DeleteObraCommand, Unit>
{
    private readonly IObraRepository _repo;
    private readonly IContratoRepository _contratoRepo;
    private readonly IOrcamentoRepository _orcamentoRepo;
    private readonly IUnitOfWork _uow;

    public DeleteObraCommandHandler(
        IObraRepository repo,
        IContratoRepository contratoRepo,
        IOrcamentoRepository orcamentoRepo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _contratoRepo = contratoRepo;
        _orcamentoRepo = orcamentoRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteObraCommand r, CancellationToken ct)
    {
        var obra = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {r.Id} não encontrada.");

        if (obra.Status == StatusObraEnum.EmAndamento)
            throw new InvalidOperationException("Não é possível excluir uma obra em andamento.");

        var (_, totalContratos) = await _contratoRepo.GetPagedAsync(r.EmpresaId, r.Id, null, 1, 1, ct);
        if (totalContratos > 0)
            throw new InvalidOperationException("Não é possível excluir uma obra com contratos vinculados.");

        var orcamentos = await _orcamentoRepo.GetByObraAsync(r.Id, r.EmpresaId, ct);
        if (orcamentos.Any())
            throw new InvalidOperationException("Não é possível excluir uma obra com orçamentos vinculados.");

        obra.IsDeleted = true;
        obra.DeletedAt = DateTime.UtcNow;
        obra.DeletedBy = r.UsuarioId;
        _repo.Update(obra);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
