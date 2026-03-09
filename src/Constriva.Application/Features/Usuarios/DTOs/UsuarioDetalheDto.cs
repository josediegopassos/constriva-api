namespace Constriva.Application.Features.Usuarios.DTOs;

public record UsuarioDetalheDto(UsuarioDto Usuario, IEnumerable<PermissaoDto> Permissoes);
