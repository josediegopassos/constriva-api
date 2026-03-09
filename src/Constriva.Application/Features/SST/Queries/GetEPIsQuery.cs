using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST;

public record GetEPIsQuery(Guid EmpresaId) : IRequest<IEnumerable<EPIDto>>, ITenantRequest;

public class GetEPIsHandler : IRequestHandler<GetEPIsQuery, IEnumerable<EPIDto>>
{
    private readonly ISSTRepository _repo;
    public GetEPIsHandler(ISSTRepository repo) => _repo = repo;

    public async Task<IEnumerable<EPIDto>> Handle(GetEPIsQuery request, CancellationToken ct)
    {
        var epis = await _repo.GetEPIsAsync(request.EmpresaId, ct);
        return epis.Select(e => new EPIDto(
            e.Id, e.Codigo, e.Nome, e.Descricao, e.Tipo,
            e.Fabricante, e.Modelo, e.NumeroCA, e.ValidadeCA,
            e.EstoqueAtual, e.EstoqueMinimo, e.VidaUtilMeses, e.Ativo));
    }
}
