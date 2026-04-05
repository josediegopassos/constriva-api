using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras;

public record GetPropostasCotacaoQuery(Guid CotacaoId, Guid EmpresaId)
    : IRequest<IEnumerable<PropostaDto>>, ITenantRequest;

public class GetPropostasCotacaoHandler
    : IRequestHandler<GetPropostasCotacaoQuery, IEnumerable<PropostaDto>>
{
    private readonly IComprasRepository _repo;
    public GetPropostasCotacaoHandler(IComprasRepository repo) => _repo = repo;

    public async Task<IEnumerable<PropostaDto>> Handle(
        GetPropostasCotacaoQuery request, CancellationToken ct)
    {
        var propostas = await _repo.GetPropostasCotacaoAsync(
            request.CotacaoId, request.EmpresaId, ct);

        return propostas.Select(PropostaMapper.ToDto);
    }
}
