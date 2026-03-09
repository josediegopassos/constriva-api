using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class Cargo : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? CBO { get; set; }
    public string? Descricao { get; set; }
    public decimal SalarioBase { get; set; }
    public decimal? SalarioMaximo { get; set; }
    public bool Ativo { get; set; } = true;
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
