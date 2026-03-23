using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Clientes.Queries;

public record GetObrasDoClienteQuery(Guid ClienteId, Guid EmpresaId)
    : IRequest<IEnumerable<ObraResumoDto>>, ITenantRequest;

public class GetObrasDoClienteHandler : IRequestHandler<GetObrasDoClienteQuery, IEnumerable<ObraResumoDto>>
{
    private readonly IObraRepository _obraRepo;
    public GetObrasDoClienteHandler(IObraRepository obraRepo) => _obraRepo = obraRepo;

    public async Task<IEnumerable<ObraResumoDto>> Handle(GetObrasDoClienteQuery r, CancellationToken ct)
    {
        var (obras, _) = await _obraRepo.GetPagedAsync(r.EmpresaId, null, null, null, 1, 1000, ct);
        var hoje = DateTime.Today;
        return obras
            .Where(o => o.ClienteId == r.ClienteId)
            .Select(o => new ObraResumoDto(
                o.Id, o.Codigo, o.Nome, o.Tipo, o.Status, o.Status.ToString(),
                o.Endereco?.Cidade, o.Endereco?.Estado, o.DataInicioPrevista, o.DataFimPrevista,
                o.ValorContrato, o.PercentualConcluido,
                o.Status == StatusObraEnum.EmAndamento && o.DataFimPrevista < hoje,
                o.FotoUrl));
    }
}
