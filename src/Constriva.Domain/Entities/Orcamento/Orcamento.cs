using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Orcamento;

public class Orcamento : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public StatusOrcamentoEnum Status { get; set; } = StatusOrcamentoEnum.Rascunho;
    public int Versao { get; set; } = 1;
    public bool ELinhaDBase { get; set; } = false;
    public DateTime DataReferencia { get; set; }
    public string? BaseOrcamentaria { get; set; }  // SINAPI Mês/Ano
    public string? Localidade { get; set; }

    // BDI
    public decimal BDI { get; set; } = 0;
    public decimal BDIDetalhado_Administracao { get; set; } = 0;
    public decimal BDIDetalhado_Seguro { get; set; } = 0;
    public decimal BDIDetalhado_Risco { get; set; } = 0;
    public decimal BDIDetalhado_Financeiro { get; set; } = 0;
    public decimal BDIDetalhado_Lucro { get; set; } = 0;
    public decimal BDIDetalhado_Tributos { get; set; } = 0;

    // Encargos Sociais
    public decimal EncargosHoristas { get; set; } = 0;
    public decimal EncargosMensalistas { get; set; } = 0;

    // Totais calculados
    public decimal ValorCustoDirecto { get; set; } = 0;
    public decimal ValorBDI { get; set; } = 0;
    public decimal ValorTotal { get; set; } = 0;

    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public string? Observacoes { get; set; }

    public virtual ICollection<GrupoOrcamento> Grupos { get; set; } = new List<GrupoOrcamento>();
    public virtual ICollection<ComposicaoOrcamento> Composicoes { get; set; } = new List<ComposicaoOrcamento>();
    public virtual ICollection<OrcamentoHistorico> Historicos { get; set; } = new List<OrcamentoHistorico>();
}
