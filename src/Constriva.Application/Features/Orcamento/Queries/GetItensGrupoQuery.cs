using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetItensGrupoQuery(
    Guid GrupoId,
    Guid EmpresaId) : IRequest<IEnumerable<ItemOrcamentoDto>>, ITenantRequest;

public class GetItensGrupoHandler
    : IRequestHandler<GetItensGrupoQuery, IEnumerable<ItemOrcamentoDto>>
{
    private readonly IItemOrcamentoRepository _repo;

    public GetItensGrupoHandler(IItemOrcamentoRepository repo) => _repo = repo;

    public async Task<IEnumerable<ItemOrcamentoDto>> Handle(
        GetItensGrupoQuery request, CancellationToken ct)
    {
        var itens = await _repo.GetByGrupoAsync(request.GrupoId, ct);
        return itens
            .Where(i => !i.IsDeleted)
            .OrderBy(i => i.Ordem)
            .Select(OrcamentoMapper.ToItemDto);
    }
}

