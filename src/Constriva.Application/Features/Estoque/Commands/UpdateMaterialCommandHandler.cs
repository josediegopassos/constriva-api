using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record UpdateMaterialCommand(Guid Id, Guid EmpresaId, string Nome, string Unidade,
    string? Categoria, string? CodigoInterno, decimal? PrecoUnitario)
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
        var material = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Material {request.Id} não encontrado.");

        material.Nome = request.Nome;
        material.UnidadeMedida = request.Unidade;
        if (request.CodigoInterno != null) material.Codigo = request.CodigoInterno;
        if (request.PrecoUnitario.HasValue) material.PrecoUltimaCompra = request.PrecoUnitario.Value;

        _repo.Update(material);
        await _uow.SaveChangesAsync(cancellationToken);

        return new MaterialDto(material.Id, material.Codigo, material.Nome, material.UnidadeMedida, material.Tipo, material.CodigoSINAPI, material.Marca);
    }
}
