using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Cronograma;

public class RecursoAtividade : TenantEntity
{
    public Guid AtividadeId { get; set; }
    public string TipoRecurso { get; set; } = null!;  // MaoDeObra, Equipamento, Material
    public string NomeRecurso { get; set; } = null!;
    public Guid? RecursoId { get; set; }
    public decimal Quantidade { get; set; }
    public string UnidadeMedida { get; set; } = null!;
    public decimal CustoUnitario { get; set; }
    public decimal CustoTotal => Quantidade * CustoUnitario;
    public virtual AtividadeCronograma Atividade { get; set; } = null!;
}
