using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateMaterialCommand(Guid EmpresaId, string Nome, string Unidade,
    string? Categoria, string? CodigoInterno, decimal? PrecoUnitario)
    : IRequest<MaterialDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreateMaterialHandler : IRequestHandler<CreateMaterialCommand, MaterialDto>
{
    private readonly IMaterialRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateMaterialHandler(IMaterialRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<MaterialDto> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var codigo = request.CodigoInterno?.Trim() ?? $"MAT-{DateTime.UtcNow:yyyyMMddHHmmss}";

        if (request.CodigoInterno != null)
        {
            var existing = await _repo.GetByCodigoAsync(request.EmpresaId, codigo, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"Já existe um material com o código interno '{codigo}'.");
        }

        var material = new Material
        {
            EmpresaId = request.EmpresaId,
            Codigo = codigo,
            Nome = request.Nome,
            UnidadeMedida = request.Unidade,
            Tipo = TipoInsumoEnum.Material,
            PrecoUltimaCompra = request.PrecoUnitario ?? 0,
            Ativo = true
        };

        await _repo.AddAsync(material, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new MaterialDto(material.Id, material.Codigo, material.Nome, material.UnidadeMedida, material.Tipo, material.CodigoSINAPI, material.Marca);
    }
}
