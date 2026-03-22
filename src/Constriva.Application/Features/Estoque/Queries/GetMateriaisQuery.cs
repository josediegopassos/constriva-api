using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque;

public record GetMateriaisQuery(Guid EmpresaId, string? Search = null) : IRequest<IEnumerable<MaterialDto>>, ITenantRequest;

public class GetMateriaisHandler : IRequestHandler<GetMateriaisQuery, IEnumerable<MaterialDto>>
{
    private readonly IMaterialRepository _repo;
    public GetMateriaisHandler(IMaterialRepository repo) => _repo = repo;

    internal static MaterialDto ToDto(Material m) => new(
        m.Id, m.Codigo, m.Nome, m.UnidadeMedida, m.Tipo,
        m.CodigoSINAPI, m.Marca, m.Fabricante,
        m.GrupoId, m.Grupo?.Nome,
        m.EstoqueMinimo, m.EstoqueMaximo,
        m.PrecoCustoMedio, m.PrecoUltimaCompra,
        m.Ativo, m.ControlaLote, m.ControlaValidade);

    public async Task<IEnumerable<MaterialDto>> Handle(GetMateriaisQuery r, CancellationToken ct)
    {
        var items = string.IsNullOrWhiteSpace(r.Search)
            ? await _repo.GetAllComGrupoAsync(r.EmpresaId, ct)
            : await _repo.SearchAsync(r.EmpresaId, r.Search, ct);
        return items.Select(ToDto);
    }
}
