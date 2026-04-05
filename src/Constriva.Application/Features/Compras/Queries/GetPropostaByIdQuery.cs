using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras;

public record GetPropostaByIdQuery(Guid PropostaId, Guid EmpresaId)
    : IRequest<PropostaDto?>, ITenantRequest;

public class GetPropostaByIdHandler : IRequestHandler<GetPropostaByIdQuery, PropostaDto?>
{
    private readonly IComprasRepository _repo;
    public GetPropostaByIdHandler(IComprasRepository repo) => _repo = repo;

    public async Task<PropostaDto?> Handle(GetPropostaByIdQuery request, CancellationToken ct)
    {
        var proposta = await _repo.GetPropostaByIdAsync(request.PropostaId, request.EmpresaId, ct);
        return proposta is null ? null : PropostaMapper.ToDto(proposta);
    }
}
