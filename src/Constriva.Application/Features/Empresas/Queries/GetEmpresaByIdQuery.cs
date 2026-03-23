using MediatR;
using Constriva.Application.Features.Empresas.DTOs;
using Constriva.Domain.Enums;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Empresas;

public record GetEmpresaByIdQuery(Guid Id) : IRequest<EmpresaDto?>;

public class GetEmpresaByIdHandler : IRequestHandler<GetEmpresaByIdQuery, EmpresaDto?>
{
    private readonly IEmpresaRepository _repo;
    private readonly IObraRepository _obraRepo;

    public GetEmpresaByIdHandler(IEmpresaRepository repo, IObraRepository obraRepo)
    {
        _repo = repo;
        _obraRepo = obraRepo;
    }

    public async Task<EmpresaDto?> Handle(GetEmpresaByIdQuery r, CancellationToken ct)
    {
        var empresa = await _repo.GetWithUsuariosAsync(r.Id, ct);
        if (empresa == null || empresa.IsDeleted) return null;

        var totalUsuarios = empresa.Usuarios.Count(u => !u.IsDeleted);
        var totalObras = await _obraRepo.CountByEmpresaAsync(r.Id, null, ct);
        return MapDto(empresa, totalUsuarios, totalObras);
    }

    private static EmpresaDto MapDto(Empresa e, int totalUsuarios, int totalObras) => new(
        e.Id, e.RazaoSocial, e.NomeFantasia, e.Cnpj, e.Email, e.Telefone, e.LogoUrl,
        e.Status, e.Plano, e.DataVencimento, e.MaxUsuarios, e.MaxObras, e.Status == StatusEmpresaEnum.Ativa,
        e.Cidade, e.Estado,
        new(e.ModuloObras, e.ModuloEstoque, e.ModuloCronograma, e.ModuloOrcamento,
            e.ModuloCompras, e.ModuloQualidade, e.ModuloContratos, e.ModuloRH,
            e.ModuloFinanceiro, e.ModuloSST, e.ModuloGED, e.ModuloRelatorios, e.ModuloClientes, e.ModuloFornecedores, e.ModuloAgente),
        totalUsuarios, totalObras, e.CreatedAt);
}
