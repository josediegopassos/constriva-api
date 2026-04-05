using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Events;

/// <summary>
/// Evento publicado quando o processamento OCR de um documento é concluído com sucesso.
/// </summary>
public record DocumentoLensCompletedEvent : IEvent
{
    /// <summary>Identificador único da mensagem.</summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>Identificador de correlação para rastreamento.</summary>
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();

    /// <summary>Data e hora de criação da mensagem.</summary>
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;

    /// <summary>Sistema de origem da mensagem.</summary>
    public string Origem { get; init; } = "Constriva.Messaging";

    /// <summary>Identificador do processamento.</summary>
    public Guid ProcessamentoId { get; init; }

    /// <summary>Identificador do usuário que solicitou o processamento.</summary>
    public Guid UsuarioId { get; init; }

    /// <summary>Identificador da obra associada (opcional).</summary>
    public Guid? ObraId { get; init; }

    /// <summary>Identificador da empresa (tenant).</summary>
    public Guid EmpresaId { get; init; }

    /// <summary>Tipo do documento detectado pelo OCR.</summary>
    public string TipoDocumento { get; init; } = string.Empty;

    /// <summary>Tipo do documento declarado pelo usuário.</summary>
    public string TipoDocumentoDeclarado { get; init; } = string.Empty;

    /// <summary>Indica se o tipo detectado confere com o declarado.</summary>
    public bool TiposConferem { get; init; }

    /// <summary>Nível de confiança do OCR (0 a 1).</summary>
    public float ConfidenceScore { get; init; }

    /// <summary>Total de itens extraídos do documento.</summary>
    public int TotalItens { get; init; }

    /// <summary>Lista de avisos gerados durante o processamento.</summary>
    public List<string> Warnings { get; init; } = new();

    /// <summary>Nome do fornecedor sugerido com base nos dados extraídos.</summary>
    public string? FornecedorSugerido { get; init; }

    /// <summary>CNPJ do fornecedor extraído do documento.</summary>
    public string? CnpjFornecedor { get; init; }

    /// <summary>Valor total extraído do documento.</summary>
    public decimal? ValorTotal { get; init; }

    /// <summary>Data de emissão extraída do documento.</summary>
    public string? DataEmissao { get; init; }

    /// <summary>Tempo total de processamento em milissegundos.</summary>
    public int TempoProcessamentoMs { get; init; }

    /// <summary>Dados completos extraídos serializados em JSON.</summary>
    public string? DadosExtraidosJson { get; init; }

    /// <summary>Método de extração utilizado: "OCR" ou "VISION_AI".</summary>
    public string MetodoExtracao { get; init; } = "OCR";
}
