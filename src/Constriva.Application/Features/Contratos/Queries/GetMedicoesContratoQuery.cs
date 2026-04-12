using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos;

public record GetMedicoesContratoQuery(Guid ContratoId, Guid EmpresaId)
    : IRequest<IEnumerable<MedicaoContratoDto>>, ITenantRequest;

public class GetMedicoesContratoHandler : IRequestHandler<GetMedicoesContratoQuery, IEnumerable<MedicaoContratoDto>>
{
    private readonly IContratoRepository _repo;
    public GetMedicoesContratoHandler(IContratoRepository repo) => _repo = repo;

    public async Task<IEnumerable<MedicaoContratoDto>> Handle(GetMedicoesContratoQuery r, CancellationToken ct)
    {
        var medicoes = await _repo.GetMedicoesAsync(r.ContratoId, r.EmpresaId, ct);
        return medicoes.Select(m => new MedicaoContratoDto(
            m.Id, m.ContratoId, m.Periodo, m.Numero,
            m.ValorMedicao, m.ValorLiquido, m.PercentualMedicao, m.Status,
            m.DataInicio, m.DataFim, m.DataSubmissao, m.DataAprovacao));
    }
}
