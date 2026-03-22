using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Estoque.Commands;

public record DeleteMovimentacaoCommand(Guid Id, Guid EmpresaId)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class DeleteMovimentacaoHandler : IRequestHandler<DeleteMovimentacaoCommand, Unit>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteMovimentacaoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteMovimentacaoCommand request, CancellationToken cancellationToken)
    {
        var mov = await _repo.GetMovimentacaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Movimentação {request.Id} não encontrada.");

        var saldo = await _repo.GetSaldoAsync(mov.AlmoxarifadoId, mov.MaterialId, request.EmpresaId, cancellationToken)
            ?? throw new InvalidOperationException("Saldo de estoque não encontrado para esta movimentação.");

        switch (mov.Tipo)
        {
            case TipoMovimentacaoEstoqueEnum.Entrada:
            case TipoMovimentacaoEstoqueEnum.Devolucao:
                if (saldo.SaldoAtual < mov.Quantidade)
                    throw new InvalidOperationException(
                        $"Não é possível excluir: saldo atual ({saldo.SaldoAtual}) é menor que a quantidade da movimentação ({mov.Quantidade}).");
                saldo.SaldoAtual -= mov.Quantidade;
                break;

            case TipoMovimentacaoEstoqueEnum.Saida:
                saldo.SaldoAtual += mov.Quantidade;
                break;

            case TipoMovimentacaoEstoqueEnum.Ajuste:
                saldo.SaldoAtual = mov.SaldoAnterior;
                break;

            case TipoMovimentacaoEstoqueEnum.Transferencia:
                saldo.SaldoAtual += mov.Quantidade;
                if (mov.AlmoxarifadoDestinoId.HasValue)
                {
                    var saldoDestino = await _repo.GetSaldoAsync(mov.AlmoxarifadoDestinoId.Value, mov.MaterialId, request.EmpresaId, cancellationToken);
                    if (saldoDestino != null)
                    {
                        if (saldoDestino.SaldoAtual < mov.Quantidade)
                            throw new InvalidOperationException(
                                $"Não é possível excluir: saldo no almoxarifado destino ({saldoDestino.SaldoAtual}) é menor que a quantidade transferida ({mov.Quantidade}).");
                        saldoDestino.SaldoAtual -= mov.Quantidade;
                        saldoDestino.UltimaMovimentacao = DateTime.UtcNow;
                    }
                }
                break;
        }

        saldo.UltimaMovimentacao = DateTime.UtcNow;
        mov.IsDeleted = true;

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
