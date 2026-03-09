using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class Departamento : TenantEntity
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public Guid? GestorId { get; set; }
    public Guid? DepartamentoPaiId { get; set; }
    public bool Ativo { get; set; } = true;
    public virtual Departamento? DepartamentoPai { get; set; }
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
