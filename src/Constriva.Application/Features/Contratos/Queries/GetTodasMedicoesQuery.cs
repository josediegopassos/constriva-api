using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Queries;

public record GetTodasMedicoesQuery(Guid EmpresaId) : IRequest<IEnumerable<MedicaoGeralDto>>, ITenantRequest;

public class GetTodasMedicoesHandler : IRequestHandler<GetTodasMedicoesQuery, IEnumerable<MedicaoGeralDto>>
{
    private readonly IContratoRepository _repo;
    public GetTodasMedicoesHandler(IContratoRepository repo) => _repo = repo;

    public async Task<IEnumerable<MedicaoGeralDto>> Handle(GetTodasMedicoesQuery r, CancellationToken ct)
    {
        var medicoes = await _repo.GetTodasMedicoesAsync(r.EmpresaId, ct);

        return medicoes.Select(m => new MedicaoGeralDto(
            m.Id, m.ContratoId,
            m.Contrato?.Numero ?? "", m.Contrato?.Fornecedor?.RazaoSocial,
            m.Periodo, m.Numero,
            m.ValorMedicao, m.ValorRetencao, m.ValorLiquido,
            m.Status, m.Status.ToString(),
            m.DataInicio, m.DataFim,
            m.DataSubmissao, m.DataAnalise, m.DataAprovacao,
            m.MotivoRejeicao, m.Observacoes, m.CreatedAt));
    }
}
