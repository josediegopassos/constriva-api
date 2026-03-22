using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record UpdateMaterialCommand(Guid Id, Guid EmpresaId, UpdateMaterialDto Dto)
    : IRequest<MaterialDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateMaterialHandler : IRequestHandler<UpdateMaterialCommand, MaterialDto>
{
    private readonly IMaterialRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateMaterialHandler(IMaterialRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<MaterialDto> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var material = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Material {request.Id} não encontrado.");

        material.Nome = dto.Nome;
        material.UnidadeMedida = dto.UnidadeMedida;
        material.Tipo = dto.Tipo;
        material.Descricao = dto.Descricao;
        material.Especificacao = dto.Especificacao;
        material.CodigoBarras = dto.CodigoBarras;
        material.CodigoSINAPI = dto.CodigoSINAPI;
        material.Marca = dto.Marca;
        material.Fabricante = dto.Fabricante;
        material.GrupoId = dto.GrupoId;
        material.EstoqueMinimo = dto.EstoqueMinimo;
        material.EstoqueMaximo = dto.EstoqueMaximo;
        material.ImagemUrl = dto.ImagemUrl;
        material.Observacoes = dto.Observacoes;
        material.ControlaLote = dto.ControlaLote;
        material.ControlaValidade = dto.ControlaValidade;
        if (dto.Codigo != null) material.Codigo = dto.Codigo;
        if (dto.PrecoCustoMedio.HasValue) material.PrecoCustoMedio = dto.PrecoCustoMedio.Value;
        if (dto.PrecoUltimaCompra.HasValue) material.PrecoUltimaCompra = dto.PrecoUltimaCompra.Value;

        _repo.Update(material);
        await _uow.SaveChangesAsync(cancellationToken);

        return GetMateriaisHandler.ToDto(material);
    }
}
