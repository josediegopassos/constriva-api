using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH;

public record GetFolhasFuncionarioQuery(Guid FuncionarioId, Guid EmpresaId)
    : IRequest<IEnumerable<FolhaFuncionarioDto>>, ITenantRequest;

public class GetFolhasFuncionarioHandler : IRequestHandler<GetFolhasFuncionarioQuery, IEnumerable<FolhaFuncionarioDto>>
{
    private readonly IRHRepository _repo;
    public GetFolhasFuncionarioHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<FolhaFuncionarioDto>> Handle(GetFolhasFuncionarioQuery r, CancellationToken ct)
    {
        var items = await _repo.GetFolhasFuncionarioAsync(r.FuncionarioId, r.EmpresaId, ct);
        return items.Select(ff => new FolhaFuncionarioDto(
            ff.Id, ff.FolhaId, ff.Folha.Competencia,
            ff.FuncionarioId, ff.Funcionario.Nome,
            ff.SalarioBruto, ff.HorasExtras, ff.ValorHorasExtras,
            ff.AdicionalNoturno, ff.Periculosidade, ff.Insalubridade,
            ff.OutrasVerbas, ff.TotalProventos,
            ff.INSS, ff.IRRF, ff.ValeTransporte, ff.ValeRefeicao,
            ff.OutrosDescontos, ff.TotalDescontos,
            ff.SalarioLiquido, ff.FGTS));
    }
}
