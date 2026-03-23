using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Clientes.DTOs;

namespace Constriva.Application.Features.Clientes.Queries;

public record GetClientesQuery(
    Guid EmpresaId, string? Search = null, StatusClienteEnum? Status = null,
    int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<ClienteResumoDto>>, ITenantRequest;

public class GetClientesHandler : IRequestHandler<GetClientesQuery, PaginatedResult<ClienteResumoDto>>
{
    private readonly IClienteRepository _repo;
    public GetClientesHandler(IClienteRepository repo) => _repo = repo;

    public async Task<PaginatedResult<ClienteResumoDto>> Handle(GetClientesQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPagedAsync(r.EmpresaId, r.Search, r.Status, r.Page, r.PageSize, ct);
        return new PaginatedResult<ClienteResumoDto>
        {
            Items = items.Select(ToResumo),
            TotalCount = total,
            Page = r.Page,
            PageSize = r.PageSize
        };
    }

    private static ClienteResumoDto ToResumo(Cliente c) => new(
        c.Id, c.Codigo, c.TipoPessoa, c.Nome, c.NomeFantasia,
        c.Documento, c.Email, c.Telefone, c.Status, c.Endereco?.Cidade, c.Endereco?.Estado);
}
