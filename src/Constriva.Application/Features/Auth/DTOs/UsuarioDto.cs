namespace Constriva.Application.Features.Auth.DTOs;

public record UsuarioDto(
    Guid Id,
    Guid? EmpresaId,
    string Nome,
    string Email,
    string? AvatarUrl,
    string Perfil,
    bool IsSuperAdmin,
    bool IsAdminEmpresa,
    string? NomeEmpresa,
    ModulosEmpresaDto? Modulos,
    IEnumerable<PermissaoDto> Permissoes
);
