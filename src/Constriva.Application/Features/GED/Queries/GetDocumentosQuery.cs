using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.GED.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.GED;

public record GetDocumentosQuery(
    Guid EmpresaId, Guid? PastaId = null, string? Search = null,
    TipoDocumentoGEDEnum? Tipo = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<DocumentoGEDDto>>, ITenantRequest;

public class GetDocumentosHandler : IRequestHandler<GetDocumentosQuery, PaginatedResult<DocumentoGEDDto>>
{
    private readonly IGEDRepository _repo;
    public GetDocumentosHandler(IGEDRepository repo) => _repo = repo;

    public async Task<PaginatedResult<DocumentoGEDDto>> Handle(GetDocumentosQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetDocumentosPagedAsync(
            r.EmpresaId, r.PastaId, r.Search, r.Tipo, r.Page, r.PageSize, ct);
        return new PaginatedResult<DocumentoGEDDto>
        {
            Items = items.Select(d =>
            {
                var arquivo = d.Arquivos.FirstOrDefault(a => a.Atual);
                return new DocumentoGEDDto(
                    d.Id, d.PastaId, d.Codigo, d.Titulo, d.Descricao,
                    d.Tipo, d.Status, d.Versao, d.NumeroRevisao, d.DataVigencia,
                    d.AprovadoPor.HasValue, d.CreatedAt,
                    arquivo?.Url, arquivo?.TamanhoBytes ?? 0);
            }),
            TotalCount = total, Page = r.Page, PageSize = r.PageSize
        };
    }
}
