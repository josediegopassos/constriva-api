using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.GED.DTOs;

namespace Constriva.Application.Features.GED;

public record GetPastasQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<IEnumerable<PastaDto>>, ITenantRequest;

public class GetPastasHandler : IRequestHandler<GetPastasQuery, IEnumerable<PastaDto>>
{
    private readonly IGEDRepository _repo;
    public GetPastasHandler(IGEDRepository repo) => _repo = repo;

    public async Task<IEnumerable<PastaDto>> Handle(GetPastasQuery r, CancellationToken ct)
    {
        var pastas = await _repo.GetPastasAsync(r.EmpresaId, r.ObraId, ct);
        return pastas.Select(p => new PastaDto(
            p.Id, p.Nome, p.Descricao, p.ObraId, p.PastaPaiId,
            p.AcessoPublico, p.Documentos.Count(d => !d.IsDeleted)));
    }
}
