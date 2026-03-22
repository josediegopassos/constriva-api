using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateMaterialCommand(Guid EmpresaId, CreateMaterialDto Dto)
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
        var dto = request.Dto;
        var codigo = dto.Codigo?.Trim() ?? $"MAT-{DateTime.UtcNow:yyyyMMddHHmmss}";

        if (dto.Codigo != null)
        {
            var existing = await _repo.GetByCodigoAsync(request.EmpresaId, codigo, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"Já existe um material com o código '{codigo}'.");
        }

        var material = new Material
        {
            EmpresaId = request.EmpresaId,
            Codigo = codigo,
            Nome = dto.Nome,
            UnidadeMedida = dto.UnidadeMedida,
            Tipo = dto.Tipo,
            Descricao = dto.Descricao,
            Especificacao = dto.Especificacao,
            CodigoBarras = dto.CodigoBarras,
            CodigoSINAPI = dto.CodigoSINAPI,
            Marca = dto.Marca,
            Fabricante = dto.Fabricante,
            GrupoId = dto.GrupoId,
            EstoqueMinimo = dto.EstoqueMinimo,
            EstoqueMaximo = dto.EstoqueMaximo,
            PrecoCustoMedio = dto.PrecoCustoMedio ?? 0,
            PrecoUltimaCompra = dto.PrecoUltimaCompra ?? 0,
            ImagemUrl = dto.ImagemUrl,
            Observacoes = dto.Observacoes,
            ControlaLote = dto.ControlaLote,
            ControlaValidade = dto.ControlaValidade,
            Ativo = true
        };

        await _repo.AddAsync(material, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return GetMateriaisHandler.ToDto(material);
    }
}
