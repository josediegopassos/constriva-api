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
    public GetMovimentacoesEstoqueHandler(IEstoqueRepository repo) => _repo = repo;

    public async Task<IEnumerable<MovimentacaoEstoqueDto>> Handle(GetMovimentacoesEstoqueQuery r, CancellationToken ct)
    {
        var movs = await _repo.GetMovimentacoesAsync(r.EmpresaId, r.AlmoxarifadoId, r.Inicio, r.Fim, ct);
        return movs.Select(m => new MovimentacaoEstoqueDto(
            m.Id, m.Tipo, m.Tipo.ToString(),
            m.Material?.Nome ?? "", m.Material?.Codigo ?? "",
            m.Quantidade, m.PrecoUnitario, m.Quantidade * m.PrecoUnitario,
            m.SaldoAnterior, m.SaldoPosterior, m.ObraId, m.CreatedAt));
    }
}
