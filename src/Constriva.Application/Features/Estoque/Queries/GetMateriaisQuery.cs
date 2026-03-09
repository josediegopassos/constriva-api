using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque;

public record GetMateriaisQuery(Guid EmpresaId, string? Search = null) : IRequest<IEnumerable<MaterialDto>>, ITenantRequest;

public class GetMateriaisHandler : IRequestHandler<GetMateriaisQuery, IEnumerable<MaterialDto>>
{
    private readonly IMaterialRepository _repo;
    public GetMateriaisHandler(IMaterialRepository repo) => _repo = repo;

    public async Task<IEnumerable<MaterialDto>> Handle(GetMateriaisQuery r, CancellationToken ct)
    {
        var items = string.IsNullOrWhiteSpace(r.Search)
            ? await _repo.GetAllByEmpresaAsync(r.EmpresaId, ct)
            : await _repo.SearchAsync(r.EmpresaId, r.Search, ct);
        return items.Where(m => !m.IsDeleted)
            .Select(m => new MaterialDto(m.Id, m.Codigo, m.Nome, m.UnidadeMedida, m.Tipo, m.CodigoSINAPI, m.Marca));
    }
}
