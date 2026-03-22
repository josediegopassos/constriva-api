using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;
using Constriva.Application.Features.Estoque.Commands;

namespace Constriva.Application.Features.Estoque.Commands;

public record AddItemRequisicaoDto(
    Guid MaterialId,
    decimal QuantidadeSolicitada,
    decimal? PrecoReferencia,
    string? Observacao);

public record AddItemRequisicaoCommand(Guid RequisicaoId, Guid EmpresaId, AddItemRequisicaoDto Dto)
    : IRequest<ItemRequisicaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class AddItemRequisicaoHandler : IRequestHandler<AddItemRequisicaoCommand, ItemRequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IMaterialRepository _materialRepo;
    private readonly IUnitOfWork _uow;

    public AddItemRequisicaoHandler(IEstoqueRepository repo, IMaterialRepository materialRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _materialRepo = materialRepo;
        _uow = uow;
    }

    public async Task<ItemRequisicaoDto> Handle(AddItemRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var requisicao = await _repo.GetRequisicaoByIdAsync(request.RequisicaoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.RequisicaoId} não encontrada.");

        var material = await _materialRepo.GetByIdAndEmpresaAsync(dto.MaterialId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Material {dto.MaterialId} não encontrado.");

        if (dto.QuantidadeSolicitada <= 0)
            throw new InvalidOperationException("Quantidade deve ser maior que zero.");

        var item = new ItemRequisicao
        {
            EmpresaId = request.EmpresaId,
            RequisicaoId = request.RequisicaoId,
            MaterialId = dto.MaterialId,
            QuantidadeSolicitada = dto.QuantidadeSolicitada,
            PrecoReferencia = dto.PrecoReferencia,
            Observacao = dto.Observacao
        };

        await _repo.AddItemRequisicaoAsync(item, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new ItemRequisicaoDto(
            item.Id,
            item.MaterialId,
            material.Codigo ?? "",
            material.Nome,
            material.UnidadeMedida,
            item.QuantidadeSolicitada,
            item.QuantidadeAtendida,
            item.PrecoReferencia,
            item.Observacao);
    }
}
