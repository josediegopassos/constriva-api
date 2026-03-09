using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record CreateItemOrcamentoCommand(Guid GrupoId, Guid EmpresaId, CreateItemOrcDto Dto)
    : IRequest<ItemOrcamentoDto>, ITenantRequest;

public class CreateItemOrcamentoHandler : IRequestHandler<CreateItemOrcamentoCommand, ItemOrcamentoDto>
{
    private readonly IItemOrcamentoRepository _itemRepo;
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IOrcamentoRepository _orcRepo;
    private readonly IUnitOfWork _uow;

    public CreateItemOrcamentoHandler(
        IItemOrcamentoRepository itemRepo, IGrupoOrcamentoRepository grupoRepo,
        IOrcamentoRepository orcRepo, IUnitOfWork uow)
    {
        _itemRepo = itemRepo;
        _grupoRepo = grupoRepo;
        _orcRepo = orcRepo;
        _uow = uow;
    }

    public async Task<ItemOrcamentoDto> Handle(CreateItemOrcamentoCommand request, CancellationToken ct)
    {
        var grupo = await _grupoRepo.GetByIdAsync(request.GrupoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        var orcamento = await _orcRepo.GetByIdAsync(grupo.OrcamentoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível adicionar itens a um orçamento aprovado.");

        var maxOrdem = await _itemRepo.GetMaxOrdemAsync(request.GrupoId, ct);
        var dto = request.Dto;

        var item = new ItemOrcamento
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = grupo.OrcamentoId,
            GrupoId = request.GrupoId,
            Codigo = $"{grupo.Codigo}.{(maxOrdem + 1):D2}",
            Descricao = dto.Descricao,
            UnidadeMedida = dto.UnidadeMedida,
            Quantidade = dto.Quantidade,
            CustoUnitario = dto.CustoUnitario,
            BDI = orcamento.BDI,
            CustoComBDI = dto.CustoUnitario * (1 + orcamento.BDI / 100),
            Ordem = maxOrdem + 1,
            Fonte = FontePrecoEnum.Manual
        };

        await _itemRepo.AddAsync(item, ct);

        grupo.ValorTotal += item.CustoTotal;
        _grupoRepo.Update(grupo);

        orcamento.ValorCustoDirecto += item.CustoTotal;
        orcamento.ValorBDI = orcamento.ValorCustoDirecto * orcamento.BDI / 100;
        orcamento.ValorTotal = orcamento.ValorCustoDirecto + orcamento.ValorBDI;
        _orcRepo.Update(orcamento);

        await _uow.SaveChangesAsync(ct);

        return OrcamentoMapper.ToItemDto(item);
    }
}
