using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque;

public record GetMovimentacoesEstoqueQuery(Guid EmpresaId, Guid? AlmoxarifadoId = null, DateTime? Inicio = null, DateTime? Fim = null)
    : IRequest<IEnumerable<MovimentacaoEstoqueDto>>, ITenantRequest;

public class GetMovimentacoesEstoqueHandler : IRequestHandler<GetMovimentacoesEstoqueQuery, IEnumerable<MovimentacaoEstoqueDto>>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public GetMovimentacoesEstoqueHandler(IEstoqueRepository repo, IUsuarioRepository usuarioRepo)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<IEnumerable<MovimentacaoEstoqueDto>> Handle(GetMovimentacoesEstoqueQuery r, CancellationToken ct)
    {
        var movs = await _repo.GetMovimentacoesAsync(r.EmpresaId, r.AlmoxarifadoId, r.Inicio, r.Fim, ct);

        var usuarioIds = movs.Select(m => m.UsuarioId).Distinct();
        var usuarios = await _usuarioRepo.FindAsync(u => usuarioIds.Contains(u.Id), ct);
        var nomes = usuarios.ToDictionary(u => u.Id, u => u.Nome);

        return movs.Select(m => new MovimentacaoEstoqueDto(
            m.Id, m.Tipo, m.Tipo.ToString(),
            m.AlmoxarifadoId, m.Almoxarifado?.Nome ?? "",
            m.MaterialId, m.Material?.Nome ?? "", m.Material?.Codigo ?? "",
            m.Quantidade, m.PrecoUnitario, m.Quantidade * m.PrecoUnitario,
            m.SaldoAnterior, m.SaldoPosterior,
            m.DataMovimentacao, m.NumeroDocumento, m.NumeroNF, m.Lote,
            m.ObraId, m.UsuarioId, nomes.GetValueOrDefault(m.UsuarioId, ""),
            m.CreatedAt));
    }
}
