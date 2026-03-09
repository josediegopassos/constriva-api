using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record CreateInsumoCommand(
    Guid ComposicaoId,
    Guid EmpresaId,
    CreateInsumoDto Dto) : IRequest<InsumoComposicaoDto>, ITenantRequest;

public class CreateInsumoHandler : IRequestHandler<CreateInsumoCommand, InsumoComposicaoDto>
{
    private readonly IComposicaoOrcamentoRepository _composicaoRepo;
    private readonly IUnitOfWork _uow;

    public CreateInsumoHandler(IComposicaoOrcamentoRepository composicaoRepo, IUnitOfWork uow)
    {
        _composicaoRepo = composicaoRepo;
        _uow = uow;
    }

    public async Task<InsumoComposicaoDto> Handle(CreateInsumoCommand request, CancellationToken ct)
    {
        var composicao = await _composicaoRepo.GetByIdAsync(request.ComposicaoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Composição não encontrada.");

        var dto = request.Dto;
        var insumo = new InsumoComposicao
        {
            EmpresaId = request.EmpresaId,
            ComposicaoId = request.ComposicaoId,
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            Tipo = dto.Tipo,
            UnidadeMedida = dto.UnidadeMedida,
            Coeficiente = dto.Coeficiente,
            PrecoUnitario = dto.PrecoUnitario,
            FontePrecoEnum = dto.FontePrecoEnum,
            MaterialId = dto.MaterialId
        };

        await _composicaoRepo.AddInsumoAsync(insumo, ct);

        composicao.CustoTotal += insumo.CustoTotal;
        _composicaoRepo.Update(composicao);

        await _uow.SaveChangesAsync(ct);

        return new InsumoComposicaoDto(
            insumo.Id, insumo.Codigo, insumo.Descricao, insumo.Tipo,
            insumo.UnidadeMedida, insumo.Coeficiente, insumo.PrecoUnitario,
            insumo.CustoTotal, insumo.FontePrecoEnum);
    }
}
