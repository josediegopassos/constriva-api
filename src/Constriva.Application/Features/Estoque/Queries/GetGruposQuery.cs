using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Estoque.Queries;

public record GrupoMaterialDto(
    Guid Id,
    string Nome,
    string? Descricao,
    Guid? GrupoPaiId,
    string? GrupoPaiNome);

public record GetGruposQuery(Guid EmpresaId) : IRequest<IEnumerable<GrupoMaterialDto>>, ITenantRequest;

public class GetGruposHandler : IRequestHandler<GetGruposQuery, IEnumerable<GrupoMaterialDto>>
{
    private readonly IEstoqueRepository _repo;
    public GetGruposHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<IEnumerable<GrupoMaterialDto>> Handle(GetGruposQuery r, CancellationToken ct)
    {
        var grupos = await _repo.GetGruposAsync(r.EmpresaId, ct);
        return grupos.Select(g => new GrupoMaterialDto(
            g.Id, g.Nome, g.Descricao, g.GrupoPaiId, g.GrupoPai?.Nome));
    }
}
