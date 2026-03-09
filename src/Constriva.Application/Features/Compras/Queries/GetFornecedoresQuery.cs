using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.Compras;

public record GetFornecedoresQuery(
    Guid EmpresaId, string? Search = null, TipoFornecedorEnum? Tipo = null,
    int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<FornecedorDto>>, ITenantRequest;

public class GetFornecedoresHandler : IRequestHandler<GetFornecedoresQuery, PaginatedResult<FornecedorDto>>
{
    private readonly IFornecedorRepository _repo;
    public GetFornecedoresHandler(IFornecedorRepository repo) => _repo = repo;

    public async Task<PaginatedResult<FornecedorDto>> Handle(GetFornecedoresQuery request, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPagedAsync(
            request.EmpresaId, request.Search, request.Tipo, request.Page, request.PageSize, ct);

        return new PaginatedResult<FornecedorDto>
        {
            Items = items.Select(f => new FornecedorDto(
                f.Id, f.RazaoSocial, f.NomeFantasia, f.Documento, null, f.Tipo,
                f.Telefone, f.Email, f.Cidade, f.Ativo)),
            TotalCount = total, Page = request.Page, PageSize = request.PageSize
        };
    }
}
