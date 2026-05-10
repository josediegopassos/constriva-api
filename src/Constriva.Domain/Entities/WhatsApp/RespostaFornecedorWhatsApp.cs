using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.WhatsApp;

public class RespostaFornecedorWhatsApp : TenantEntity
{
    public Guid CotacaoWhatsAppId { get; private set; }
    public Guid FornecedorCotacaoId { get; private set; }
    public Guid FornecedorId { get; private set; }
    public Guid? PropostaCotacaoId { get; private set; }
    public string WaMessageId { get; private set; } = null!;
    public string TelefoneOrigem { get; private set; } = null!;
    public DateTime RecebidaEm { get; private set; }
    public TipoConteudoMensagemEnum TipoConteudo { get; private set; }
    public string? TextoMensagem { get; private set; }
    public string? WaMediaId { get; private set; }
    public string? MediaUrl { get; private set; }
    public string? MediaMimeType { get; private set; }
    public string? MediaNomeArquivo { get; private set; }
    public string? MediaPathArmazenado { get; private set; }
    public string PayloadWebhookOriginal { get; private set; } = null!;
    public bool ProcessadoPelaIa { get; private set; }
    public DateTime? ProcessadaEm { get; private set; }
    public int? NivelConfianca { get; private set; }
    public bool ExtraidaComSucesso { get; private set; }
    public MotivoFalhaProcessamentoEnum? MotivoFalha { get; private set; }
    public string? DescricaoFalha { get; private set; }
    public int TentativasProcessamento { get; private set; }

    // ── Navigation Properties ─────────────────────────────────────────────
    public virtual CotacaoWhatsApp CotacaoWhatsApp { get; private set; } = null!;

    // ── Constructors ──────────────────────────────────────────────────────
    private RespostaFornecedorWhatsApp() { }

    public RespostaFornecedorWhatsApp(
        Guid empresaId,
        Guid cotacaoWhatsAppId,
        Guid fornecedorCotacaoId,
        Guid fornecedorId,
        string waMessageId,
        string telefoneOrigem,
        DateTime recebidaEm,
        TipoConteudoMensagemEnum tipoConteudo,
        string payloadWebhookOriginal,
        string? textoMensagem = null,
        string? waMediaId = null,
        string? mediaUrl = null,
        string? mediaMimeType = null,
        string? mediaNomeArquivo = null)
    {
        EmpresaId = empresaId;
        CotacaoWhatsAppId = cotacaoWhatsAppId;
        FornecedorCotacaoId = fornecedorCotacaoId;
        FornecedorId = fornecedorId;
        WaMessageId = waMessageId;
        TelefoneOrigem = telefoneOrigem;
        RecebidaEm = recebidaEm;
        TipoConteudo = tipoConteudo;
        PayloadWebhookOriginal = payloadWebhookOriginal;
        TextoMensagem = textoMensagem;
        WaMediaId = waMediaId;
        MediaUrl = mediaUrl;
        MediaMimeType = mediaMimeType;
        MediaNomeArquivo = mediaNomeArquivo;
        ProcessadoPelaIa = false;
        ExtraidaComSucesso = false;
        TentativasProcessamento = 0;
    }

    // ── Domain Methods ────────────────────────────────────────────────────

    public void RegistrarMediaArmazenada(string mediaPath)
    {
        MediaPathArmazenado = mediaPath;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoExtraidaComSucesso(Guid propostaCotacaoId, int nivelConfianca)
    {
        PropostaCotacaoId = propostaCotacaoId;
        ProcessadoPelaIa = true;
        ProcessadaEm = DateTime.UtcNow;
        NivelConfianca = nivelConfianca;
        ExtraidaComSucesso = true;
        TentativasProcessamento++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoFalhaNaExtracao(
        MotivoFalhaProcessamentoEnum motivo,
        string descricaoFalha,
        int? nivelConfiancaObtido = null)
    {
        ProcessadoPelaIa = true;
        ProcessadaEm = DateTime.UtcNow;
        NivelConfianca = nivelConfiancaObtido;
        ExtraidaComSucesso = false;
        MotivoFalha = motivo;
        DescricaoFalha = descricaoFalha;
        TentativasProcessamento++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void PrepararParaReprocessamento()
    {
        ProcessadoPelaIa = false;
        ProcessadaEm = null;
        ExtraidaComSucesso = false;
        MotivoFalha = null;
        DescricaoFalha = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
