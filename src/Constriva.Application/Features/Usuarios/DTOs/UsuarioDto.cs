using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Usuarios.DTOs;

public record UsuarioDto(Guid Id, string Nome, string Email, PerfilUsuarioEnum Perfil, string PerfilLabel, string? Telefone, string? Cargo, string? AvatarUrl, bool Ativo, bool IsSuperAdmin, bool IsAdminEmpresa, Guid? EmpresaId, DateTime? UltimoAcesso, DateTime CreatedAt);
