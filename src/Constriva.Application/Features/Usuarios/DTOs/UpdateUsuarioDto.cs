using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Usuarios.DTOs;

public record UpdateUsuarioDto(string Nome, string? Telefone, string? Cargo, PerfilUsuarioEnum Perfil);
