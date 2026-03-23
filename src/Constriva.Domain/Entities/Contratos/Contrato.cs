using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Contratos;

public class Contrato : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid FornecedorId { get; set; }
    public string Numero { get; set; } = null!;
    public string Objeto { get; set; } = null!;
    public string? Descricao { get; set; }
    public TipoContratoFornecedorEnum Tipo { get; set; }
    public StatusContratoEnum Status { get; set; } = StatusContratoEnum.Rascunho;
    public DateTime DataAssinatura { get; set; }
    public DateTime DataVigenciaInicio { get; set; }
    public DateTime DataVigenciaFim { get; set; }
    public DateTime? DataEncerramento { get; set; }
    public decimal ValorGlobal { get; set; }
    public decimal ValorAditivos { get; set; } = 0;
    public decimal ValorTotal => ValorGlobal + ValorAditivos;
    public decimal ValorMedidoAcumulado { get; set; } = 0;
    public decimal ValorPagoAcumulado { get; set; } = 0;
    public decimal PercentualRetencao { get; set; } = 0;
    public decimal ValorRetencao { get; set; } = 0;
    public string? CondicoesPagamento { get; set; }
    public int? DiasParaMedicao { get; set; }
    public int? DiasParaPagamento { get; set; }
    public string? Observacoes { get; set; }
    public string? ArquivoUrl { get; set; }
    public Guid? AssinadoPor { get; set; }
    public Guid? FiscalId { get; set; }

    public virtual Fornecedor Fornecedor { get; set; } = null!;
    public virtual ICollection<AditvoContrato> Aditivos { get; set; } = new List<AditvoContrato>();
    public virtual ICollection<MedicaoContratual> Medicoes { get; set; } = new List<MedicaoContratual>();
    public virtual ICollection<FaturaContrato> Faturas { get; set; } = new List<FaturaContrato>();
    public virtual ICollection<ContratoAnexo> Anexos { get; set; } = new List<ContratoAnexo>();
}
