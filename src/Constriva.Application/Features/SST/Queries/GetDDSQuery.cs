using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.SST;

public record GetDDSQuery(Guid EmpresaId, Guid? ObraId = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<DDSDto>>, ITenantRequest;

public class GetDDSHandler : IRequestHandler<GetDDSQuery, PaginatedResult<DDSDto>>
{
    private readonly ISSTRepository _repo;
    public GetDDSHandler(ISSTRepository repo) => _repo = repo;

    public async Task<PaginatedResult<DDSDto>> Handle(GetDDSQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetDDSPagedAsync(r.EmpresaId, r.ObraId, r.Page, r.PageSize, ct);
        return new PaginatedResult<DDSDto>
        {
            Items = items.Select(d => new DDSDto(
                d.Id, d.ObraId, d.Numero, d.Tema, d.Conteudo,
                d.Ministrador, d.NumeroParticipantes, d.DataRealizacao, d.CreatedAt)),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
