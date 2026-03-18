using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Queries;

public record GetObraByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<ObraDto?>, ITenantRequest;

public class GetObraByIdHandler : IRequestHandler<GetObraByIdQuery, ObraDto?>
{
    private readonly IObraRepository _repo;
    public GetObraByIdHandler(IObraRepository repo) => _repo = repo;

    public async Task<ObraDto?> Handle(GetObraByIdQuery r, CancellationToken ct)
    {
        var o = await _repo.GetWithFasesAsync(r.Id, r.EmpresaId, ct);
        if (o == null || o.IsDeleted) return null;

        return new ObraDto(
            o.Id, o.Codigo, o.Nome, o.Descricao, o.Tipo, o.TipoContrato, o.Status,
            o.ClienteId, o.NomeCliente, o.ResponsavelTecnico, o.CreaResponsavel,
            o.NumeroART, o.NumeroRRT, o.NumeroAlvara, o.ValidadeAlvara,
            o.AreaTotal, o.AreaConstruida, o.NumeroAndares, o.NumeroUnidades,
            o.DataInicioPrevista, o.DataFimPrevista, o.DataInicioReal, o.DataFimReal,
            o.ValorContrato, o.ValorOrcado, o.ValorRealizado, o.PercentualConcluido,
            o.Logradouro, o.Numero, o.Complemento, o.Bairro, o.Cidade, o.Estado, o.Cep,
            o.Latitude, o.Longitude, o.Observacoes, o.FotoUrl, o.CreatedAt,
            o.Fases.Where(f => !f.IsDeleted).OrderBy(f => f.Ordem)
                .Select(f => new FaseObraDto(
                    f.Id, f.Nome, f.Descricao, f.Ordem, f.Status,
                    f.PercentualConcluido,
                    f.DataInicioPrevista, f.DataFimPrevista,
                    f.DataInicioReal, f.DataFimReal,
                    f.ValorPrevisto, f.ValorRealizado,
                    f.FasePaiId, f.Cor)));
    }
}
