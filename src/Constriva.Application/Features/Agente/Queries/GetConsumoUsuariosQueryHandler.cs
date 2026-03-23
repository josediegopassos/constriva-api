using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Agente.DTOs;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Queries;

public record GetConsumoUsuariosQuery(Guid EmpresaId)
    : IRequest<IEnumerable<ConsumoUsuarioDto>>, ITenantRequest;

public class GetConsumoUsuariosHandler : IRequestHandler<GetConsumoUsuariosQuery, IEnumerable<ConsumoUsuarioDto>>
{
    private readonly IAgenteRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;
    public GetConsumoUsuariosHandler(IAgenteRepository repo, IUsuarioRepository usuarioRepo)
    { _repo = repo; _usuarioRepo = usuarioRepo; }

    public async Task<IEnumerable<ConsumoUsuarioDto>> Handle(GetConsumoUsuariosQuery r, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var items = await _repo.GetTopUsuariosAsync(r.EmpresaId, now.Year, now.Month, 10, ct);

        var result = new List<ConsumoUsuarioDto>();
        foreach (var item in items)
        {
            var usuario = await _usuarioRepo.GetByIdAsync(item.UsuarioId, ct);
            var nome = usuario?.Nome ?? "Usuário desconhecido";
            result.Add(new ConsumoUsuarioDto(
                item.UsuarioId, nome, item.TokensUtilizados, item.TotalRequisicoes));
        }

        return result;
    }
}
