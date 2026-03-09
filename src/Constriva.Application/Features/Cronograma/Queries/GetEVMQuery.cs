using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma;

public record GetEVMQuery(Guid ObraId, Guid EmpresaId) : IRequest<EVMDto>, ITenantRequest;

public class GetEVMHandler : IRequestHandler<GetEVMQuery, EVMDto>
{
    private readonly ICronogramaRepository _repo;
    public GetEVMHandler(ICronogramaRepository repo) => _repo = repo;

    public async Task<EVMDto> Handle(GetEVMQuery request, CancellationToken ct)
    {
        var crono = await _repo.GetWithAtividadesAsync(request.ObraId, request.EmpresaId, ct);
        if (crono == null)
            return new EVMDto(0, 0, 0, 0, 0, 1, 1, 0, 0);

        // Cálculos EVM básicos (apenas atividades não deletadas)
        var atividades = crono.Atividades.Where(a => !a.IsDeleted).ToList();
        var vp = atividades.Sum(a => a.BCWS);
        var va = atividades.Sum(a => a.BCWP);
        var cr = atividades.Sum(a => a.ACWP);
        var vc = va - cr;
        var varPrazo = va - vp;
        var idc = cr > 0 ? va / cr : 1;
        var idp = vp > 0 ? va / vp : 1;
        var et = idc > 0 ? cr + Math.Max(0, vp - va) / idc : vp;
        var vt = vp - et;

        return new EVMDto(vp, va, cr, vc, varPrazo, idc, idp, et, vt);
    }
}
