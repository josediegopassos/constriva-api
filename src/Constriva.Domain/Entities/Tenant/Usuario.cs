using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Tenant;

public class Usuario : BaseEntity
{
    public Guid? EmpresaId { get; set; }   // null = SuperAdmin plataforma
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Cargo { get; set; }
    public string? AvatarUrl { get; set; }
    public bool Ativo { get; set; } = true;
    public bool IsSuperAdmin { get; set; } = false;
    public bool IsAdminEmpresa { get; set; } = false;
    public PerfilUsuarioEnum Perfil { get; set; } = PerfilUsuarioEnum.Colaborador;
    public DateTime? UltimoAcesso { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public bool EmailConfirmado { get; set; } = false;
    public string? TokenConfirmacaoEmail { get; set; }
    public string? TokenRedefinicaoSenha { get; set; }
    public DateTime? TokenRedefinicaoExpiry { get; set; }
    public int TentativasLogin { get; set; } = 0;
    public DateTime? BloqueadoAte { get; set; }
    public string? TimeZone { get; set; } = "America/Sao_Paulo";
    public string? Idioma { get; set; } = "pt-BR";

    public virtual Empresa? Empresa { get; set; }
    public virtual ICollection<UsuarioPermissao> Permissoes { get; set; } = new List<UsuarioPermissao>();
    public virtual ICollection<UsuarioObra> ObrasVinculadas { get; set; } = new List<UsuarioObra>();
    public virtual ICollection<LogAcesso> LogsAcesso { get; set; } = new List<LogAcesso>();
}
