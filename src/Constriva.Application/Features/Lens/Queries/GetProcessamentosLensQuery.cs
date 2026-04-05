using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Extensions;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetProcessamentosLensQuery(
    Guid EmpresaId,
    Guid? ObraId = null,
    StatusProcessamentoLensEnum? Status = null,
    TipoDocumentoLensEnum? TipoDocumento = null,
    DateTime? De = null,
    DateTime? Ate = null,
    int Pagina = 1,
    int TamanhoPagina = 20)
    : IRequest<PaginatedResult<ProcessamentoLensDto>>, ITenantRequest;

public class GetProcessamentosLensHandler : IRequestHandler<GetProcessamentosLensQuery, PaginatedResult<ProcessamentoLensDto>>
{
    private readonly IDocumentoLensRepository _repo;
    private readonly IRepository<Domain.Entities.Tenant.Usuario> _usuarioRepo;

    public GetProcessamentosLensHandler(
        IDocumentoLensRepository repo,
        IRepository<Domain.Entities.Tenant.Usuario> usuarioRepo)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<PaginatedResult<ProcessamentoLensDto>> Handle(GetProcessamentosLensQuery r, CancellationToken ct)
    {
        var (docs, total) = await _repo.GetProcessamentosPagedAsync(
            r.EmpresaId, r.ObraId, r.Status, r.TipoDocumento, r.De, r.Ate,
            r.Pagina, r.TamanhoPagina, ct);

        var usuarioIds = docs.Select(d => d.UsuarioId).Distinct().ToList();
        var usuariosList = await _usuarioRepo.FindAsync(u => usuarioIds.Contains(u.Id), ct);
        var usuarios = usuariosList.ToDictionary(u => u.Id, u => u.Nome);

        var items = docs.Select(d => MapToDto(d, usuarios));

        return new PaginatedResult<ProcessamentoLensDto>
        {
            Items = items,
            TotalCount = total,
            Page = r.Pagina,
            PageSize = r.TamanhoPagina
        };
    }

    private static ProcessamentoLensDto MapToDto(DocumentoLens d, Dictionary<Guid, string> usuarios) => new(
        d.Id,
        d.TipoDocumento,
        d.TipoDocumento.ToDescricao(),
        d.Status,
        d.Status.ToString(),
        d.NomeArquivo,
        d.ExtensaoArquivo,
        d.ConfidenceScore,
        d.Itens.Count,
        d.Itens.Count(i => i.Status == StatusItemLensEnum.Aprovado),
        d.Itens.Count(i => i.Status == StatusItemLensEnum.Rejeitado),
        d.Itens.Count(i => i.Status == StatusItemLensEnum.Pendente),
        d.NomeFornecedorExtraido,
        d.ValorTotalExtraido,
        d.DataEmissaoExtraida,
        d.Observacoes,
        d.MetodoExtracao,
        d.MetodoExtracao == MetodoExtracaoLensEnum.VisionAI ? "GPT-4o mini (Vision AI)" : "OCR (Tesseract)",
        d.CreatedAt,
        d.UpdatedAt,
        usuarios.GetValueOrDefault(d.UsuarioId, "Usuário"));
}
