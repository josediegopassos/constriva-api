using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.Qualidade;

public record GetInspecoesQuery(
    Guid EmpresaId, Guid? ObraId = null, StatusInspecaoEnum? Status = null,
    int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<InspecaoDto>>, ITenantRequest;

public class GetInspecoesHandler : IRequestHandler<GetInspecoesQuery, PaginatedResult<InspecaoDto>>
{
    private readonly IQualidadeRepository _repo;
    public GetInspecoesHandler(IQualidadeRepository repo) => _repo = repo;

    public async Task<PaginatedResult<InspecaoDto>> Handle(GetInspecoesQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetInspecoesPagedAsync(
            r.EmpresaId, r.ObraId, r.Status, r.Page, r.PageSize, ct);
        return new PaginatedResult<InspecaoDto>
        {
            Items = items.Select(i => new InspecaoDto(
                i.Id, i.ObraId, i.Numero, i.Titulo, i.Descricao,
                i.Status, i.DataProgramada, i.DataRealizacao,
                i.Localicacao, i.TemNaoConformidade, i.CreatedAt)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
