using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.WhatsApp;

public class CotacaoWhatsApp : TenantEntity
{
    public Guid CotacaoId { get; private set; }
    public string TelefoneEmpresa { get; private set; } = null!;
    public string NomeExibicaoEmpresa { get; private set; } = null!;
    public DateTime? DisparadaEm { get; private set; }
    public DateTime DataLimiteResposta { get; private set; }
    public int TotalFornecedoresConvidados { get; private set; }
    public int TotalRespostas { get; private set; }
    public int TotalPropostasExtraidas { get; private set; }
    public DateTime? EncerradaEm { get; private set; }
    public string? MensagemPersonalizada { get; private set; }

    // ── Navigation Properties ─────────────────────────────────────────────
    public virtual ICollection<MensagemWhatsApp> Mensagens { get; private set; } = null!;
    public virtual ICollection<RespostaFornecedorWhatsApp> Respostas { get; private set; } = null!;

    // ── Constructors ──────────────────────────────────────────────────────
    private CotacaoWhatsApp() { }

    public CotacaoWhatsApp(
        Guid empresaId,
        Guid cotacaoId,
        string telefoneEmpresa,
        string nomeExibicaoEmpresa,
        DateTime dataLimiteResposta,
        string? mensagemPersonalizada)
    {
        EmpresaId = empresaId;
        CotacaoId = cotacaoId;
        TelefoneEmpresa = telefoneEmpresa;
        NomeExibicaoEmpresa = nomeExibicaoEmpresa;
        DataLimiteResposta = dataLimiteResposta;
        MensagemPersonalizada = mensagemPersonalizada;
        TotalFornecedoresConvidados = 0;
        TotalRespostas = 0;
        TotalPropostasExtraidas = 0;
        Mensagens = new List<MensagemWhatsApp>();
        Respostas = new List<RespostaFornecedorWhatsApp>();
    }

    // ── Domain Methods ────────────────────────────────────────────────────

    public void MarcarComoDisparada(int totalFornecedores)
    {
        DisparadaEm = DateTime.UtcNow;
        TotalFornecedoresConvidados = totalFornecedores;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementarRespostas()
    {
        TotalRespostas++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementarPropostasExtraidas()
    {
        TotalPropostasExtraidas++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Encerrar()
    {
        EncerradaEm = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool PrazoExpirado() => DateTime.UtcNow > DataLimiteResposta;
}
