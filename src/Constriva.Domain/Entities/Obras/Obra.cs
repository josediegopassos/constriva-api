using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Obras;

public class Obra : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public TipoObraEnum Tipo { get; set; }
    public StatusObraEnum Status { get; set; } = StatusObraEnum.Orcamento;
    public TipoContratoObraEnum TipoContrato { get; set; }
    public Guid? ClienteId { get; set; }
    public string? NomeCliente { get; set; }
    public string? ResponsavelTecnico { get; set; }
    public string? CreaResponsavel { get; set; }
    public string? NumeroART { get; set; }
    public string? NumeroRRT { get; set; }
    public string? NumeroAlvara { get; set; }
    public DateTime? ValidadeAlvara { get; set; }
    public double? AreaTotal { get; set; }
    public double? AreaConstruida { get; set; }
    public int? NumeroAndares { get; set; }
    public int? NumeroUnidades { get; set; }
    public DateTime DataInicioPrevista { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public DateTime? DataInicioReal { get; set; }
    public DateTime? DataFimReal { get; set; }
    public decimal ValorContrato { get; set; }
    public decimal ValorOrcado { get; set; }
    public decimal ValorRealizado { get; set; }
    public decimal PercentualConcluido { get; set; } = 0;
    public string? FotoUrl { get; set; }
    public string? Observacoes { get; set; }

    // Localização
    public Guid? EnderecoId { get; set; }
    public virtual Endereco? Endereco { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<FaseObra> Fases { get; set; } = new List<FaseObra>();
    public virtual ICollection<ObraAnexo> Anexos { get; set; } = new List<ObraAnexo>();
    public virtual ICollection<ObraHistorico> Historicos { get; set; } = new List<ObraHistorico>();
    public virtual ICollection<Diario.RDO> RDOs { get; set; } = new List<Diario.RDO>();
}
