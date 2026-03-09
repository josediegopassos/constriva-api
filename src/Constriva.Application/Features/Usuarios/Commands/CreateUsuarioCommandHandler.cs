using MediatR;
using BCrypt.Net;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Usuarios.DTOs;

namespace Constriva.Application.Features.Usuarios.Commands;

public record CreateUsuarioCommand(bool RequesterIsSuperAdmin, Guid? RequesterEmpresaId, CreateUsuarioDto Dto) : IRequest<UsuarioDto>;

public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, UsuarioDto>
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnitOfWork _uow;

    public CreateUsuarioHandler(IUsuarioRepository usuarioRepo, IUnitOfWork uow)
    {
        _usuarioRepo = usuarioRepo;
        _uow = uow;
    }

    public async Task<UsuarioDto> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var email = dto.Email.Trim().ToLowerInvariant();

        // Não-super-admins só podem criar usuários dentro da própria empresa
        if (!request.RequesterIsSuperAdmin && dto.EmpresaId != request.RequesterEmpresaId)
            throw new UnauthorizedAccessException("Sem permissão para criar usuário nesta empresa.");

        // Super-admins verificam unicidade global (e-mail único na plataforma).
        // Usuários comuns verificam apenas dentro do próprio tenant, permitindo o mesmo
        // e-mail em empresas diferentes (ex.: franquias com mesmo colaborador).
        bool emailExists;
        if (request.RequesterIsSuperAdmin)
        {
            emailExists = await _usuarioRepo.EmailExistsAsync(email, null, cancellationToken);
        }
        else
        {
            var empresaId = dto.EmpresaId ?? request.RequesterEmpresaId ?? Guid.Empty;
            var existing = await _usuarioRepo.GetByEmailAndEmpresaAsync(email, empresaId, cancellationToken);
            emailExists = existing != null;
        }
        if (emailExists)
            throw new InvalidOperationException($"E-mail já cadastrado: {email}");

        var usuario = new Usuario
        {
            Nome = dto.Nome.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Telefone = dto.Telefone,
            Cargo = dto.Cargo,
            Perfil = dto.Perfil,
            EmpresaId = dto.EmpresaId,
            Ativo = true
        };

        await _usuarioRepo.AddAsync(usuario, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new UsuarioDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.Perfil,
            usuario.Perfil.ToString(), usuario.Telefone, usuario.Cargo,
            null, usuario.Ativo, usuario.IsSuperAdmin, usuario.IsAdminEmpresa,
            usuario.EmpresaId, null, usuario.CreatedAt);
    }
}
