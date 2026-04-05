using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Lens;

public class DocumentoLens : TenantEntity
{
    public Guid? ObraId { get; set; }
    public Guid? CentroCustoId { get; set; }
    public Guid? FornecedorId { get; set; }
    public Guid UsuarioId { get; set; }
    public TipoDocumentoLensEnum TipoDocumento { get; set; }
    public TipoDocumentoLensEnum TipoDocumentoDeclarado { get; set; }
    public bool TiposConferem { get; set; }
    public StatusProcessamentoLensEnum Status { get; set; } = StatusProcessamentoLensEnum.Pendente;
    public string NomeArquivo { get; set; } = string.Empty;
    public string CaminhoArquivo { get; set; } = string.Empty;
    public string ExtensaoArquivo { get; set; } = string.Empty;
    public long TamanhoBytes { get; set; }
    public float? ConfidenceScore { get; set; }
    public string? TextoBruto { get; set; }
    public List<string> Warnings { get; set; } = new();
    public string? MensagemErro { get; set; }
    public string? CodigoErro { get; set; }
    public bool PodeReprocessar { get; set; }
    public int TentativaNumero { get; set; } = 1;
    public int? TempoProcessamentoMs { get; set; }
    public int? PaginasProcessadas { get; set; }
    public string? Observacoes { get; set; }
    public string? MotivoRejeicaoOuCancelamento { get; set; }
    public string? NumeroDocumentoExtraido { get; set; }
    public string? DataEmissaoExtraida { get; set; }
    public decimal? ValorTotalExtraido { get; set; }
    public string? CnpjFornecedorExtraido { get; set; }
    public string? NomeFornecedorExtraido { get; set; }
    public Guid? CompraId { get; set; }
    public MetodoExtracaoLensEnum MetodoExtracao { get; set; } = MetodoExtracaoLensEnum.OCR;

    public virtual List<ItemDocumentoLens> Itens { get; set; } = new();
}
