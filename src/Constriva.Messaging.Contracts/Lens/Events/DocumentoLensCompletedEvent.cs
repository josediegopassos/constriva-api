using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Events;

public record DocumentoLensCompletedEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";
    public Guid ProcessamentoId { get; init; }
    public Guid UsuarioId { get; init; }
    public Guid? ObraId { get; init; }
    public Guid EmpresaId { get; init; }
    public string TipoDocumento { get; init; } = string.Empty;
    public string TipoDocumentoDeclarado { get; init; } = string.Empty;
    public bool TiposConferem { get; init; }
    public float ConfidenceScore { get; init; }
    public int TotalItens { get; init; }
    public List<string> Warnings { get; init; } = new();
    public string? FornecedorSugerido { get; init; }
    public string? CnpjFornecedor { get; init; }
    public decimal? ValorTotal { get; init; }
    public string? DataEmissao { get; init; }
    public int TempoProcessamentoMs { get; init; }
    public string? DadosExtraidosJson { get; init; }
    public string MetodoExtracao { get; init; } = "OCR";
}
