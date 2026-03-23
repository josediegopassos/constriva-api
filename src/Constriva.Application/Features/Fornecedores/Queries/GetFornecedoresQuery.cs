using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Fornecedores.DTOs;
using Constriva.Application.Features.Fornecedores.Commands;

namespace Constriva.Application.Features.Fornecedores.Queries;

public record GetFornecedoresQuery(
    Guid EmpresaId, string? Search = null, TipoFornecedorEnum? Tipo = null,
    int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<FornecedorResumoDto>>, ITenantRequest;

public class GetFornecedoresHandler : IRequestHandler<GetFornecedoresQuery, PaginatedResult<FornecedorResumoDto>>
{
    private readonly IFornecedorRepository _repo;
    public GetFornecedoresHandler(IFornecedorRepository repo) => _repo = repo;

    public async Task<PaginatedResult<FornecedorResumoDto>> Handle(GetFornecedoresQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPagedAsync(r.EmpresaId, r.Search, r.Tipo, r.Page, r.PageSize, ct);

        return new PaginatedResult<FornecedorResumoDto>
        {
            Items = items.Select(CreateFornecedorCommandHandler.ToResumo),
            TotalCount = total,
            Page = r.Page,
            PageSize = r.PageSize
        };
    }
}
