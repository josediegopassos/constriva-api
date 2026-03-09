using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque;

public record GetAlmoxarifadosQuery(Guid EmpresaId) : IRequest<IEnumerable<AlmoxarifadoDto>>, ITenantRequest;

public class GetAlmoxarifadosHandler : IRequestHandler<GetAlmoxarifadosQuery, IEnumerable<AlmoxarifadoDto>>
{
    private readonly IEstoqueRepository _repo;
    public GetAlmoxarifadosHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<IEnumerable<AlmoxarifadoDto>> Handle(GetAlmoxarifadosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetAlmoxarifadosAsync(r.EmpresaId, ct);
        return items.Select(a => new AlmoxarifadoDto(a.Id, a.Nome, a.Descricao ?? "", a.ObraId, a.Principal));
    }
}
