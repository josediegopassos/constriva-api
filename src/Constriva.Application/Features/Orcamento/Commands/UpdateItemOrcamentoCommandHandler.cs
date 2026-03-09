using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record UpdateItemOrcamentoCommand(Guid Id, Guid EmpresaId, UpdateItemOrcDto Dto)
    : IRequest<ItemOrcamentoDto>, ITenantRequest;

public class UpdateItemOrcamentoHandler : IRequestHandler<UpdateItemOrcamentoCommand, ItemOrcamentoDto>
{
    private readonly IItemOrcamentoRepository _itemRepo;
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IOrcamentoRepository _orcRepo;
    private readonly IUnitOfWork _uow;

    public UpdateItemOrcamentoHandler(
        IItemOrcamentoRepository itemRepo, IGrupoOrcamentoRepository grupoRepo,
        IOrcamentoRepository orcRepo, IUnitOfWork uow)
    {
        _itemRepo = itemRepo;
        _grupoRepo = grupoRepo;
        _orcRepo = orcRepo;
        _uow = uow;
    }

    public async Task<ItemOrcamentoDto> Handle(UpdateItemOrcamentoCommand request, CancellationToken ct)
    {
        var item = await _itemRepo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Item não encontrado.");

        var oldCusto = item.CustoTotal;

        var grupo = await _grupoRepo.GetByIdAsync(item.GrupoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        var orcamento = await _orcRepo.GetByIdAsync(item.OrcamentoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        var dto = request.Dto;
        item.Descricao = dto.Descricao;
        item.UnidadeMedida = dto.UnidadeMedida;
        item.Quantidade = dto.Quantidade;
        item.CustoUnitario = dto.CustoUnitario;
        item.CustoComBDI = dto.CustoUnitario * (1 + item.BDI / 100);

        var diff = item.CustoTotal - oldCusto;

        _itemRepo.Update(item);

        grupo.ValorTotal += diff;
        _grupoRepo.Update(grupo);

        orcamento.ValorCustoDirecto += diff;
        orcamento.ValorBDI = orcamento.ValorCustoDirecto * orcamento.BDI / 100;
        orcamento.ValorTotal = orcamento.ValorCustoDirecto + orcamento.ValorBDI;
        _orcRepo.Update(orcamento);

        await _uow.SaveChangesAsync(ct);

        return OrcamentoMapper.ToItemDto(item);
    }
}
