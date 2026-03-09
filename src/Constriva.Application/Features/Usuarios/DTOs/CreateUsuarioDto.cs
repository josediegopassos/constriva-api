using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Usuarios.DTOs;

public record CreateUsuarioDto(string Nome, string Email, string Senha, string? Telefone, string? Cargo, PerfilUsuarioEnum Perfil, Guid? EmpresaId);
