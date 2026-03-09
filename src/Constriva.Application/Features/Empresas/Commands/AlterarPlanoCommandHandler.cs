using FluentValidation;
using MediatR;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Empresas.DTOs;

namespace Constriva.Application.Features.Empresas.Commands;

public record AlterarPlanoCommand(Guid EmpresaId, PlanoEmpresaEnum NovoPlano, int MaxUsuarios, int MaxObras, DateTime DataVencimento, string? Motivo) : IRequest<bool>;

public class AlterarPlanoHandler : IRequestHandler<AlterarPlanoCommand, bool>
{
    private readonly IEmpresaRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IUnitOfWork _uow;

    public AlterarPlanoHandler(IEmpresaRepository repo, IObraRepository obraRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _obraRepo = obraRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(AlterarPlanoCommand request, CancellationToken cancellationToken)
    {
        if (request.MaxUsuarios <= 0)
            throw new ArgumentException("MaxUsuarios deve ser maior que zero.");
        if (request.MaxObras <= 0)
            throw new ArgumentException("MaxObras deve ser maior que zero.");
        if (request.DataVencimento.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("DataVencimento não pode ser uma data passada.");

        // Carrega empresa com usuários para checar uso atual
        var empresa = await _repo.GetWithUsuariosAsync(request.EmpresaId, cancellationToken);
        if (empresa == null || empresa.IsDeleted) return false;

        var usuariosAtivos = empresa.Usuarios.Count(u => !u.IsDeleted && u.Ativo);
        if (request.MaxUsuarios < usuariosAtivos)
            throw new InvalidOperationException(
                $"Não é possível reduzir o limite para {request.MaxUsuarios} usuários. A empresa possui {usuariosAtivos} usuários ativos.");

        // Valida downgrade de obras: não permitir reduzir abaixo do uso atual
        var obrasCount = await _obraRepo.CountByEmpresaAsync(request.EmpresaId, null, cancellationToken);
        if (request.MaxObras < obrasCount)
            throw new InvalidOperationException(
                $"Não é possível reduzir o limite para {request.MaxObras} obras. A empresa possui {obrasCount} obras cadastradas.");

        empresa.Plano = request.NovoPlano;
        empresa.MaxUsuarios = request.MaxUsuarios;
        empresa.MaxObras = request.MaxObras;
        empresa.DataVencimento = request.DataVencimento;

        _repo.Update(empresa);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
