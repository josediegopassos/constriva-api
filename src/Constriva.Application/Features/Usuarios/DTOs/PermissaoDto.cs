namespace Constriva.Application.Features.Usuarios.DTOs;

public record PermissaoDto(Guid Id, string Modulo, bool PodeVisualizar, bool PodeCriar, bool PodeEditar, bool PodeDeletar, bool PodeAprovar, bool PodeExportar, bool PodeAdministrar);
