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

public record UpdateUsuarioCommand(Guid Id, Guid? RequesterEmpresaId, bool RequesterIsSuperAdmin, UpdateUsuarioDto Dto) : IRequest<bool>;

public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioCommand, bool>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;

    public UpdateUsuarioHandler(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
    {
        _usuarioRepo = usuarioRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(request.Id, cancellationToken);
        if (usuario == null || usuario.IsDeleted) return false;

        if (!request.RequesterIsSuperAdmin && request.RequesterEmpresaId != usuario.EmpresaId)
            throw new UnauthorizedAccessException("Sem permissão para editar este usuário.");

        var dto = request.Dto;

        // Anti-escalação de privilégios
        if (!request.RequesterIsSuperAdmin)
        {
            if (dto.Perfil == PerfilUsuarioEnum.SuperAdmin)
                throw new UnauthorizedAccessException("Sem permissão para atribuir perfil SuperAdmin.");

            // Apenas super-admins podem promover para AdminEmpresa quem ainda não o é
            if (dto.Perfil == PerfilUsuarioEnum.AdminEmpresa && usuario.Perfil != PerfilUsuarioEnum.AdminEmpresa)
                throw new UnauthorizedAccessException("Sem permissão para promover usuário a administrador da empresa.");
        }

        usuario.Nome = dto.Nome.Trim();
        if (dto.Telefone != null) usuario.Telefone = dto.Telefone.Trim();
        if (dto.Cargo != null) usuario.Cargo = dto.Cargo.Trim();
        usuario.Perfil = dto.Perfil;

        _usuarioRepo.Update(usuario);
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
