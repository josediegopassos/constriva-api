using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Queries;

public record GetSessaoByIdQuery(Guid SessaoId, Guid EmpresaId)
    : IRequest<SessaoDetalheDto?>, ITenantRequest;

public class GetSessaoByIdHandler : IRequestHandler<GetSessaoByIdQuery, SessaoDetalheDto?>
{
    private readonly IAgenteRepository _repo;
    public GetSessaoByIdHandler(IAgenteRepository repo) => _repo = repo;

    public async Task<SessaoDetalheDto?> Handle(GetSessaoByIdQuery r, CancellationToken ct)
    {
        var sessao = await _repo.GetSessaoComHistoricoAsync(r.SessaoId, r.EmpresaId, ct);
        if (sessao == null) return null;

        var mensagens = sessao.Mensagens
            .OrderBy(m => m.CreatedAt)
            .Select(m => new MensagemDto(
                m.Role.ToString(), m.Conteudo,
                m.TokensInput, m.TokensOutput, m.CreatedAt));

        return new SessaoDetalheDto(sessao.Id, sessao.AtualizadaEm, mensagens);
    }
}
