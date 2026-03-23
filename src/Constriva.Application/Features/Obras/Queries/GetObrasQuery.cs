using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Queries;

public record GetObrasQuery(
    Guid EmpresaId,
    int Page = 1, int PageSize = 20,
    string? Search = null,
    StatusObraEnum? Status = null,
    TipoObraEnum? Tipo = null)
    : IRequest<PaginatedResult<ObraResumoDto>>, ITenantRequest;

public class GetObrasHandler : IRequestHandler<GetObrasQuery, PaginatedResult<ObraResumoDto>>
{
    private readonly IObraRepository _repo;
    public GetObrasHandler(IObraRepository repo) => _repo = repo;

    public async Task<PaginatedResult<ObraResumoDto>> Handle(GetObrasQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPagedAsync(
            r.EmpresaId, r.Search, r.Status, r.Tipo, r.Page, r.PageSize, ct);

        var hoje = DateTime.Today;
        return new PaginatedResult<ObraResumoDto>
        {
            Items = items.Select(o => new ObraResumoDto(
                o.Id, o.Codigo, o.Nome, o.Tipo, o.Status, o.Status.ToString(),
                o.Endereco?.Cidade, o.Endereco?.Estado, o.DataInicioPrevista, o.DataFimPrevista,
                o.ValorContrato, o.PercentualConcluido,
                o.Status == StatusObraEnum.EmAndamento && o.DataFimPrevista < hoje,
                o.FotoUrl)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}

