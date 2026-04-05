using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Constriva.Messaging.Models.Lens;

/// <summary>
/// Documento MongoDB para registro de log de processamento OCR via Constriva.Lens.
/// </summary>
public class LogProcessamentoLens
{
    /// <summary>Identificador MongoDB do documento.</summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    /// <summary>Identificador do processamento no PostgreSQL.</summary>
    [BsonElement("processamento_id")]
    public Guid ProcessamentoId { get; set; }

    /// <summary>Identificador do usuário que solicitou o processamento.</summary>
    [BsonElement("usuario_id")]
    public Guid UsuarioId { get; set; }

    /// <summary>Identificador da empresa (tenant).</summary>
    [BsonElement("empresa_id")]
    public Guid EmpresaId { get; set; }

    /// <summary>Identificador da obra associada (opcional).</summary>
    [BsonElement("obra_id")]
    public Guid? ObraId { get; set; }

    /// <summary>Tipo do documento detectado pelo OCR.</summary>
    [BsonElement("tipo_documento")]
    public string TipoDocumento { get; set; } = string.Empty;

    /// <summary>Tipo do documento declarado pelo usuário.</summary>
    [BsonElement("tipo_documento_declarado")]
    public string TipoDocumentoDeclarado { get; set; } = string.Empty;

    /// <summary>Indica se o tipo detectado confere com o declarado.</summary>
    [BsonElement("tipos_conferem")]
    public bool TiposConferem { get; set; }

    /// <summary>Status do processamento.</summary>
    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Nível de confiança do OCR (0 a 1).</summary>
    [BsonElement("confidence_score")]
    public float ConfidenceScore { get; set; }

    /// <summary>Texto bruto extraído pelo OCR.</summary>
    [BsonElement("texto_bruto")]
    public string TextoBruto { get; set; } = string.Empty;

    /// <summary>Total de itens extraídos.</summary>
    [BsonElement("total_itens")]
    public int TotalItens { get; set; }

    /// <summary>Lista de avisos gerados durante o processamento.</summary>
    [BsonElement("warnings")]
    public List<string> Warnings { get; set; } = new();

    /// <summary>Lista de erros ocorridos.</summary>
    [BsonElement("erros")]
    public List<string> Erros { get; set; } = new();

    /// <summary>Tempo de processamento em milissegundos.</summary>
    [BsonElement("tempo_processamento_ms")]
    public int TempoProcessamentoMs { get; set; }

    /// <summary>Número de páginas processadas.</summary>
    [BsonElement("paginas_processadas")]
    public int PaginasProcessadas { get; set; }

    /// <summary>Tamanho do arquivo em bytes.</summary>
    [BsonElement("tamanho_arquivo_bytes")]
    public long TamanhoArquivoBytes { get; set; }

    /// <summary>Extensão do arquivo processado.</summary>
    [BsonElement("extensao_arquivo")]
    public string ExtensaoArquivo { get; set; } = string.Empty;

    /// <summary>URL do Constriva.Lens utilizada para o processamento.</summary>
    [BsonElement("url_lens_utilizada")]
    public string UrlLensUtilizada { get; set; } = string.Empty;

    /// <summary>Número da tentativa de processamento.</summary>
    [BsonElement("tentativa_numero")]
    public int TentativaNumero { get; set; } = 1;

    /// <summary>Data e hora de criação (utilizada como índice TTL).</summary>
    [BsonElement("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    /// <summary>Data e hora da última atualização.</summary>
    [BsonElement("atualizado_em")]
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}
