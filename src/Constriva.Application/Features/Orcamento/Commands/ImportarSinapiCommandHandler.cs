using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record ImportarSinapiCommand(Guid OrcamentoId, Guid EmpresaId, ImportarSinapiDto Dto)
    : IRequest<int>, ITenantRequest;

public class ImportarSinapiHandler : IRequestHandler<ImportarSinapiCommand, int>
{
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IItemOrcamentoRepository _itemRepo;
    private readonly IOrcamentoRepository _orcRepo;
    private readonly IUnitOfWork _uow;

    public ImportarSinapiHandler(
        IGrupoOrcamentoRepository grupoRepo, IItemOrcamentoRepository itemRepo,
        IOrcamentoRepository orcRepo, IUnitOfWork uow)
    {
        _grupoRepo = grupoRepo;
        _itemRepo = itemRepo;
        _orcRepo = orcRepo;
        _uow = uow;
    }

    public async Task<int> Handle(ImportarSinapiCommand request, CancellationToken ct)
    {
        var dto = request.Dto;
        if (!dto.Itens.Any()) return 0;

        var orcamento = await _orcRepo.GetByIdAsync(request.OrcamentoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível importar itens em orçamento aprovado.");

        GrupoOrcamento? grupo = null;
        if (dto.GrupoId.HasValue)
            grupo = await _grupoRepo.GetByIdAsync(dto.GrupoId.Value, request.EmpresaId, ct);

        // Check for duplicate SINAPI codes already in this orcamento
        var existingItems = await _itemRepo.GetByOrcamentoAsync(request.OrcamentoId, ct);
        var existingCodigos = existingItems.Select(i => i.CodigoFonte).Where(c => c != null).ToHashSet();

        // Compute maxOrdem once before the loop (avoids N+1 DB calls)
        int nextOrdem = grupo != null
            ? await _itemRepo.GetMaxOrdemAsync(grupo.Id, ct) + 1
            : 0;

        int count = 0;
        foreach (var sinapiItem in dto.Itens)
        {
            if (existingCodigos.Contains(sinapiItem.Codigo))
                continue; // skip duplicates

            var item = new ItemOrcamento
            {
                EmpresaId = request.EmpresaId,
                OrcamentoId = request.OrcamentoId,
                GrupoId = grupo?.Id ?? Guid.Empty,
                Codigo = sinapiItem.Codigo,
                CodigoFonte = sinapiItem.Codigo,
                Descricao = sinapiItem.Descricao,
                UnidadeMedida = sinapiItem.UnidadeMedida,
                CustoUnitario = sinapiItem.CustoUnitario,
                Quantidade = sinapiItem.Quantidade,
                BDI = orcamento.BDI,
                CustoComBDI = sinapiItem.CustoUnitario * (1 + orcamento.BDI / 100),
                Ordem = nextOrdem++,
                Fonte = FontePrecoEnum.SINAPI
            };

            await _itemRepo.AddAsync(item, ct);

            if (grupo != null)
            {
                grupo.ValorTotal += item.CustoTotal;
                _grupoRepo.Update(grupo);
            }

            orcamento.ValorCustoDirecto += item.CustoTotal;
            count++;
        }

        orcamento.ValorBDI = orcamento.ValorCustoDirecto * orcamento.BDI / 100;
        orcamento.ValorTotal = orcamento.ValorCustoDirecto + orcamento.ValorBDI;
        _orcRepo.Update(orcamento);

        await _uow.SaveChangesAsync(ct);
        return count;
    }
}
