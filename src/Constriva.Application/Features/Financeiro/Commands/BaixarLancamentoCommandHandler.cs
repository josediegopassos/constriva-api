using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Commands;

public record BaixarLancamentoCommand(Guid Id, Guid EmpresaId, decimal ValorRealizado, DateTime DataPagamento)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class BaixarLancamentoHandler : IRequestHandler<BaixarLancamentoCommand, Unit>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    private readonly IUnitOfWork _uow;

    public BaixarLancamentoHandler(ILancamentoFinanceiroRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(BaixarLancamentoCommand request, CancellationToken cancellationToken)
    {
        var lancamento = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Lançamento {request.Id} não encontrado.");

        if (lancamento.Status == StatusLancamentoEnum.Realizado)
            throw new InvalidOperationException("Lançamento já foi baixado.");

        if (request.ValorRealizado <= 0)
            throw new ArgumentException("O valor realizado deve ser positivo.");

        if (request.ValorRealizado > lancamento.Valor)
            throw new InvalidOperationException(
                $"Valor realizado ({request.ValorRealizado:C}) não pode exceder o valor do lançamento ({lancamento.Valor:C}).");

        lancamento.Status = StatusLancamentoEnum.Realizado;
        lancamento.ValorRealizado = request.ValorRealizado;
        lancamento.DataPagamento = request.DataPagamento;

        _repo.Update(lancamento);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
