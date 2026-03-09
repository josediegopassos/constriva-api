using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record DeleteItemOrcamentoCommand(Guid Id, Guid EmpresaId) : IRequest<bool>, ITenantRequest;

public class DeleteItemOrcamentoHandler : IRequestHandler<DeleteItemOrcamentoCommand, bool>
{
    private readonly IItemOrcamentoRepository _itemRepo;
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IOrcamentoRepository _orcRepo;
    private readonly IUnitOfWork _uow;

    public DeleteItemOrcamentoHandler(
        IItemOrcamentoRepository itemRepo,
        IGrupoOrcamentoRepository grupoRepo,
        IOrcamentoRepository orcRepo,
        IUnitOfWork uow)
    {
        _itemRepo = itemRepo;
        _grupoRepo = grupoRepo;
        _orcRepo = orcRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteItemOrcamentoCommand request, CancellationToken ct)
    {
        var item = await _itemRepo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Item não encontrado.");

        var grupo = await _grupoRepo.GetByIdAsync(item.GrupoId, request.EmpresaId, ct);
        var orcamento = await _orcRepo.GetByIdAsync(item.OrcamentoId, request.EmpresaId, ct);

        if (orcamento?.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível remover itens de um orçamento aprovado.");

        var custo = item.CustoTotal;
        _itemRepo.Remove(item);

        if (grupo != null)
        {
            grupo.ValorTotal = Math.Max(0, grupo.ValorTotal - custo);
            _grupoRepo.Update(grupo);
        }

        if (orcamento != null)
        {
            orcamento.ValorCustoDirecto = Math.Max(0, orcamento.ValorCustoDirecto - custo);
            orcamento.ValorBDI = orcamento.ValorCustoDirecto * orcamento.BDI / 100;
            orcamento.ValorTotal = orcamento.ValorCustoDirecto + orcamento.ValorBDI;
            _orcRepo.Update(orcamento);
        }

        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
