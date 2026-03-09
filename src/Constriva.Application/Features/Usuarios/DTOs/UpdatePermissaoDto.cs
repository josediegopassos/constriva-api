namespace Constriva.Application.Features.Usuarios.DTOs;

public record UpdatePermissaoDto(string Modulo, bool PodeVisualizar, bool PodeCriar, bool PodeEditar, bool PodeDeletar, bool PodeAprovar, bool PodeExportar, bool PodeAdministrar);
