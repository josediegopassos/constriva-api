using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Commands;

public record DeleteLancamentoCommand(Guid Id, Guid EmpresaId)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class DeleteLancamentoHandler : IRequestHandler<DeleteLancamentoCommand, Unit>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteLancamentoHandler(ILancamentoFinanceiroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteLancamentoCommand request, CancellationToken cancellationToken)
    {
        var lancamento = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Lançamento {request.Id} não encontrado.");

        lancamento.IsDeleted = true;
        _repo.Update(lancamento);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
