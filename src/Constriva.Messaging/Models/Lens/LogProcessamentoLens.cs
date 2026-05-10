using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Constriva.Messaging.Models.Lens;

public class LogProcessamentoLens
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("processamento_id")]
    public Guid ProcessamentoId { get; set; }

    [BsonElement("usuario_id")]
    public Guid UsuarioId { get; set; }

    [BsonElement("empresa_id")]
    public Guid EmpresaId { get; set; }

    [BsonElement("obra_id")]
    public Guid? ObraId { get; set; }

    [BsonElement("tipo_documento")]
    public string TipoDocumento { get; set; } = string.Empty;

    [BsonElement("tipo_documento_declarado")]
    public string TipoDocumentoDeclarado { get; set; } = string.Empty;

    [BsonElement("tipos_conferem")]
    public bool TiposConferem { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    [BsonElement("confidence_score")]
    public float ConfidenceScore { get; set; }

    [BsonElement("texto_bruto")]
    public string TextoBruto { get; set; } = string.Empty;

    [BsonElement("total_itens")]
    public int TotalItens { get; set; }

    [BsonElement("warnings")]
    public List<string> Warnings { get; set; } = new();

    [BsonElement("erros")]
    public List<string> Erros { get; set; } = new();

    [BsonElement("tempo_processamento_ms")]
    public int TempoProcessamentoMs { get; set; }

    [BsonElement("paginas_processadas")]
    public int PaginasProcessadas { get; set; }

    [BsonElement("tamanho_arquivo_bytes")]
    public long TamanhoArquivoBytes { get; set; }

    [BsonElement("extensao_arquivo")]
    public string ExtensaoArquivo { get; set; } = string.Empty;

    [BsonElement("url_lens_utilizada")]
    public string UrlLensUtilizada { get; set; } = string.Empty;

    [BsonElement("tentativa_numero")]
    public int TentativaNumero { get; set; } = 1;

    [BsonElement("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [BsonElement("atualizado_em")]
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}
