using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Financeiro;

public class AnexoLancamento : TenantEntity
{
    public Guid LancamentoId { get; set; }
    public string Nome { get; set; } = null!;
    public string Url { get; set; } = null!;
    public long TamanhoBytes { get; set; }
    public string TipoArquivo { get; set; } = null!;
    public virtual LancamentoFinanceiro Lancamento { get; set; } = null!;
}
