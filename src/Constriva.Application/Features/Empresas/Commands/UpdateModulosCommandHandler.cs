using FluentValidation;
using MediatR;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Empresas.DTOs;

namespace Constriva.Application.Features.Empresas.Commands;

public record UpdateModulosCommand(Guid EmpresaId, UpdateModulosDto Dto) : IRequest<bool>;

public class UpdateModulosHandler : IRequestHandler<UpdateModulosCommand, bool>
{
    private readonly IEmpresaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateModulosHandler(IEmpresaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(UpdateModulosCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _repo.GetByIdAsync(request.EmpresaId, cancellationToken);
        if (empresa == null) return false;

        var dto = request.Dto;
        empresa.ModuloObras = dto.Obras;
        empresa.ModuloEstoque = dto.Estoque;
        empresa.ModuloCronograma = dto.Cronograma;
        empresa.ModuloOrcamento = dto.Orcamento;
        empresa.ModuloCompras = dto.Compras;
        empresa.ModuloQualidade = dto.Qualidade;
        empresa.ModuloContratos = dto.Contratos;
        empresa.ModuloRH = dto.RH;
        empresa.ModuloFinanceiro = dto.Financeiro;
        empresa.ModuloSST = dto.SST;
        empresa.ModuloGED = dto.GED;
        empresa.ModuloClientes = dto.Clientes;
        empresa.ModuloRelatorios = dto.Relatorios;

        _repo.Update(empresa);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
