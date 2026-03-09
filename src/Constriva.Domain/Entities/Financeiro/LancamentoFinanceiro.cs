using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Financeiro;

public class LancamentoFinanceiro : TenantEntity
{
    public Guid? ObraId { get; set; }
    public Guid? CentroCustoId { get; set; }
    public Guid? ContaBancariaId { get; set; }
    public Guid? PlanoContaId { get; set; }
    public Guid? FornecedorId { get; set; }
    public Guid? ContratoId { get; set; }
    public Guid? PedidoCompraId { get; set; }
    public string Descricao { get; set; } = null!;
    public TipoLancamentoEnum Tipo { get; set; }
    public StatusLancamentoEnum Status { get; set; } = StatusLancamentoEnum.Previsto;
    public decimal Valor { get; set; }
    public decimal? ValorRealizado { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public DateTime? DataCompetencia { get; set; }
    public FormaPagamentoEnum? FormaPagamentoEnum { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? NumeroNF { get; set; }
    public bool Repetir { get; set; } = false;
    public string? Periodicidade { get; set; }
    public int? QtdParcelas { get; set; }
    public int? NumeroParcela { get; set; }
    public Guid? LancamentoPaiId { get; set; }
    public string? Observacoes { get; set; }
    public string? ComprovantePagUrl { get; set; }
    public StatusAprovacaoPagamentoEnum StatusAprovacao { get; set; } = StatusAprovacaoPagamentoEnum.Pendente;
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public Guid CriadoPor { get; set; }

    public virtual CentroCusto? CentroCusto { get; set; }
    public virtual ContaBancaria? ContaBancaria { get; set; }
    public virtual PlanoContas? PlanoContas { get; set; }
    public virtual ICollection<AnexoLancamento> Anexos { get; set; } = new List<AnexoLancamento>();
}
