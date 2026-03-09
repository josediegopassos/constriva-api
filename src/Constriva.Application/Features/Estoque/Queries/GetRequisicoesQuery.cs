using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.Estoque;

public record GetRequisicoesQuery(Guid EmpresaId, Guid? ObraId = null, StatusRequisicaoEnum? Status = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<RequisicaoDto>>, ITenantRequest;

public class GetRequisicoesHandler : IRequestHandler<GetRequisicoesQuery, PaginatedResult<RequisicaoDto>>
{
    private readonly IEstoqueRepository _repo;
    public GetRequisicoesHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<PaginatedResult<RequisicaoDto>> Handle(GetRequisicoesQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetRequisicoesPagedAsync(r.EmpresaId, r.ObraId, r.Status, r.Page, r.PageSize, ct);
        return new PaginatedResult<RequisicaoDto>
        {
            Items = items.Select(req => new RequisicaoDto(
                req.Id, req.Numero, req.ObraId, req.AlmoxarifadoId,
                req.Motivo, req.Status, req.Status.ToString(), req.SolicitanteId, req.CreatedAt)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
