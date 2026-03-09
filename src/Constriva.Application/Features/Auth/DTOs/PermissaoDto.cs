namespace Constriva.Application.Features.Auth.DTOs;

public record PermissaoDto(
    string Modulo, bool Visualizar, bool Criar, bool Editar, bool Deletar, bool Aprovar, bool Exportar, bool Administrar
);
