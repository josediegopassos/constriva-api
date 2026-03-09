using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.GED;

public class DocumentoGED : TenantEntity
{
    public Guid? ObraId { get; set; }
    public Guid PastaId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public TipoDocumentoGEDEnum Tipo { get; set; }
    public StatusDocumentoGEDEnum Status { get; set; } = StatusDocumentoGEDEnum.Rascunho;
    public string? Versao { get; set; }
    public int NumeroRevisao { get; set; } = 0;
    public string? NormaReferencia { get; set; }
    public string? Palavraschave { get; set; }
    public DateTime? DataVigencia { get; set; }
    public DateTime? DataCadastro { get; set; } = DateTime.UtcNow;
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public bool ControleAcesso { get; set; } = false;

    public virtual PastaDocumento Pasta { get; set; } = null!;
    public virtual ICollection<ArquivoDocumento> Arquivos { get; set; } = new List<ArquivoDocumento>();
    public virtual ICollection<RevisaoDocumento> Revisoes { get; set; } = new List<RevisaoDocumento>();
    public virtual ICollection<FluxoAprovacaoDoc> FluxoAprovacao { get; set; } = new List<FluxoAprovacaoDoc>();
    public virtual ICollection<DistribuicaoDoc> Distribuicoes { get; set; } = new List<DistribuicaoDoc>();
    public virtual ICollection<AcessoDocumento> Acessos { get; set; } = new List<AcessoDocumento>();
}
