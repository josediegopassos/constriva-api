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

public record AtivarDesativarUsuarioCommand(Guid Id, Guid? RequesterEmpresaId, bool RequesterIsSuperAdmin, bool Ativo) : IRequest<bool>;

public class AtivarDesativarUsuarioHandler : IRequestHandler<AtivarDesativarUsuarioCommand, bool>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;

    public AtivarDesativarUsuarioHandler(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
    {
        _usuarioRepo = usuarioRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(AtivarDesativarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(request.Id, cancellationToken);
        if (usuario == null)
            return false;

        usuario.Ativo = request.Ativo;
        await _uow.SaveChangesAsync(cancellationToken);
        return true;
    }
}
