namespace Constriva.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid? EmpresaId { get; }
    string Email { get; }
    bool IsSuperAdmin { get; }
    bool IsAdminEmpresa { get; }
    string Perfil { get; }
    bool HasPermission(string modulo, string acao);
}
