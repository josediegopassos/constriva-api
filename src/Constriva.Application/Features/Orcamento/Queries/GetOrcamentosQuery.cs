using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Orcamento.Commands;

namespace Constriva.Application.Features.Orcamento.Queries;

public record GetOrcamentosQuery(
    Guid EmpresaId,
    Guid? ObraId = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PaginatedResult<OrcamentoResumoDto>>, ITenantRequest;

public class GetOrcamentosHandler : IRequestHandler<GetOrcamentosQuery, PaginatedResult<OrcamentoResumoDto>>
{
    private readonly IOrcamentoRepository _repo;
    public GetOrcamentosHandler(IOrcamentoRepository repo) => _repo = repo;

    public async Task<PaginatedResult<OrcamentoResumoDto>> Handle(GetOrcamentosQuery request, CancellationToken ct)
    {
        var total = await _repo.CountByEmpresaAsync(request.EmpresaId, request.ObraId, ct);
        var items = await _repo.GetByEmpresaAsync(request.EmpresaId, request.ObraId, request.Page, request.PageSize, ct);

        var dtos = items.Select(o =>
        {
            var totalGrupos = o.Grupos?.Count(g => !g.IsDeleted) ?? 0;
            return OrcamentoMapper.ToResumoDto(o, totalGrupos);
        });

        return new PaginatedResult<OrcamentoResumoDto>
        {
            Items      = dtos,
            TotalCount = total,
            Page       = request.Page,
            PageSize   = request.PageSize
        };
    }
}
