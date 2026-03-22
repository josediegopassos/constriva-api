using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Estoque.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Estoque.Queries;

public record GetMaterialByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<MaterialDetalheDto?>, ITenantRequest;

public class GetMaterialByIdHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDetalheDto?>
{
    private readonly IMaterialRepository _repo;
    public GetMaterialByIdHandler(IMaterialRepository repo) => _repo = repo;

    public async Task<MaterialDetalheDto?> Handle(GetMaterialByIdQuery r, CancellationToken ct)
    {
        var m = await _repo.GetWithGrupoAsync(r.Id, r.EmpresaId, ct);
        if (m == null || m.IsDeleted) return null;

        return new MaterialDetalheDto(
            m.Id, m.Codigo, m.Nome, m.Descricao, m.Especificacao,
            m.UnidadeMedida, m.CodigoBarras, m.CodigoSINAPI, m.Marca, m.Fabricante,
            m.Tipo, m.GrupoId, m.Grupo?.Nome,
            m.EstoqueMinimo, m.EstoqueMaximo, m.PrecoCustoMedio, m.PrecoUltimaCompra,
            m.Ativo, m.ImagemUrl, m.Observacoes,
            m.ControlaLote, m.ControlaValidade, m.CreatedAt);
    }
}
