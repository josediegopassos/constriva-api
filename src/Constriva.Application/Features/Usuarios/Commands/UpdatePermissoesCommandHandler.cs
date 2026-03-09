using FluentValidation;
using MediatR;
using BCrypt.Net;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Usuarios.DTOs;

namespace Constriva.Application.Features.Usuarios.Commands;

public record UpdatePermissoesCommand(Guid UsuarioId, Guid? RequesterEmpresaId, bool RequesterIsSuperAdmin, List<UpdatePermissaoDto> Permissoes, bool ConfirmarRemocaoTotal = false) : IRequest<bool>;

public class UpdatePermissoesHandler : IRequestHandler<UpdatePermissoesCommand, bool>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;

    public UpdatePermissoesHandler(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
    {
        _usuarioRepo = usuarioRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(UpdatePermissoesCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario == null || usuario.IsDeleted) return false;

        if (!request.RequesterIsSuperAdmin && request.RequesterEmpresaId != usuario.EmpresaId)
            throw new UnauthorizedAccessException("Sem permissão para alterar permissões deste usuário.");

        // Proteger remoção de acessos críticos de administradores:
        // se o alvo é AdminEmpresa e a nova lista remove todos os PodeAdministrar,
        // exige confirmação explícita de um super-admin.
        if (usuario.IsAdminEmpresa && !request.RequesterIsSuperAdmin
            && !request.Permissoes.Any(p => p.PodeAdministrar)
            && !request.ConfirmarRemocaoTotal)
            throw new InvalidOperationException(
                "Remover todos os acessos administrativos requer confirmação explícita de um super administrador.");

        var permissoes = request.Permissoes.Select(p => new UsuarioPermissao
        {
            UsuarioId = request.UsuarioId,
            EmpresaId = usuario.EmpresaId ?? Guid.Empty,
            Modulo = p.Modulo,
            PodeVisualizar = p.PodeVisualizar,
            PodeCriar = p.PodeCriar,
            PodeEditar = p.PodeEditar,
            PodeDeletar = p.PodeDeletar,
            PodeAprovar = p.PodeAprovar,
            PodeExportar = p.PodeExportar,
            PodeAdministrar = p.PodeAdministrar
        });

        await _usuarioRepo.ReplacePermissoesAsync(request.UsuarioId, usuario.EmpresaId ?? Guid.Empty, permissoes, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
