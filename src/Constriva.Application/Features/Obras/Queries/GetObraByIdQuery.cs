using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
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
            o.NomeCliente, o.ResponsavelTecnico,
            o.DataInicioPrevista, o.DataFimPrevista, o.DataInicioReal, o.DataFimReal,
            o.ValorContrato, o.ValorOrcado, o.ValorRealizado, o.PercentualConcluido,
            o.Logradouro, o.Numero, o.Complemento, o.Bairro, o.Cidade, o.Estado, o.Cep,
            o.Observacoes, o.FotoUrl,
            o.Fases.Where(f => !f.IsDeleted).OrderBy(f => f.Ordem)
                .Select(f => new FaseObraDto(
                    f.Id, f.Nome, f.Ordem, f.PercentualConcluido,
                    f.DataInicioPrevista, f.DataFimPrevista, f.ValorPrevisto, f.FasePaiId)));
    }
}

