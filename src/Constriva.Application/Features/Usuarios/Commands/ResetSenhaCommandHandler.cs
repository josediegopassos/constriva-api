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

public record ResetSenhaCommand(Guid Id, Guid? RequesterEmpresaId, bool RequesterIsSuperAdmin, string NovaSenha) : IRequest<bool>;

public class ResetSenhaHandler : IRequestHandler<ResetSenhaCommand, bool>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;

    public ResetSenhaHandler(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
    {
        _usuarioRepo = usuarioRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(ResetSenhaCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(request.Id, cancellationToken);
        if (usuario == null || usuario.IsDeleted) return false;

        if (!request.RequesterIsSuperAdmin && request.RequesterEmpresaId != usuario.EmpresaId)
            throw new UnauthorizedAccessException("Sem permissão para redefinir a senha deste usuário.");

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NovaSenha);

        // Invalidar sessões ativas: força re-login com a nova senha
        usuario.RefreshToken = null;
        usuario.RefreshTokenExpiry = null;

        _usuarioRepo.Update(usuario);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
