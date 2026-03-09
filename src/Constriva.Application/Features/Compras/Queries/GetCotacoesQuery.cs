using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras;

public record GetCotacoesQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<IEnumerable<CotacaoDto>>, ITenantRequest;

public class GetCotacoesHandler : IRequestHandler<GetCotacoesQuery, IEnumerable<CotacaoDto>>
{
    private readonly IComprasRepository _repo;
    public GetCotacoesHandler(IComprasRepository repo) => _repo = repo;

    public async Task<IEnumerable<CotacaoDto>> Handle(GetCotacoesQuery request, CancellationToken ct)
    {
        var cotacoes = await _repo.GetCotacoesAsync(request.EmpresaId, request.ObraId, ct);
        return cotacoes.Select(c => new CotacaoDto(c.Id, c.Numero, c.ObraId, c.DataLimiteResposta ?? DateTime.MinValue, 0m, c.Status));
    }
}
