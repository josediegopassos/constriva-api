using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.WhatsApp;

public class MensagemWhatsApp : TenantEntity
{
    public Guid CotacaoWhatsAppId { get; private set; }
    public Guid FornecedorCotacaoId { get; private set; }
    public Guid FornecedorId { get; private set; }
    public string TelefoneDestino { get; private set; } = null!;
    public string NomeFornecedor { get; private set; } = null!;
    public TipoMensagemWhatsAppEnum TipoMensagem { get; private set; }
    public StatusEnvioWhatsAppEnum Status { get; private set; }
    public string? WaMessageId { get; private set; }
    public DateTime? EnviadaEm { get; private set; }
    public DateTime? EntregueEm { get; private set; }
    public DateTime? LidaEm { get; private set; }
    public int NumeroTentativa { get; private set; }
    public string? ErroEnvio { get; private set; }
    public string PayloadEnviado { get; private set; } = null!;

    // ── Navigation Properties ─────────────────────────────────────────────
    public virtual CotacaoWhatsApp CotacaoWhatsApp { get; private set; } = null!;

    // ── Constructors ──────────────────────────────────────────────────────
    private MensagemWhatsApp() { }

    public MensagemWhatsApp(
        Guid empresaId,
        Guid cotacaoWhatsAppId,
        Guid fornecedorCotacaoId,
        Guid fornecedorId,
        string telefoneDestino,
        string nomeFornecedor,
        TipoMensagemWhatsAppEnum tipoMensagem,
        string payloadEnviado,
        int numeroTentativa = 1)
    {
        EmpresaId = empresaId;
        CotacaoWhatsAppId = cotacaoWhatsAppId;
        FornecedorCotacaoId = fornecedorCotacaoId;
        FornecedorId = fornecedorId;
        TelefoneDestino = telefoneDestino;
        NomeFornecedor = nomeFornecedor;
        TipoMensagem = tipoMensagem;
        PayloadEnviado = payloadEnviado;
        NumeroTentativa = numeroTentativa;
        Status = StatusEnvioWhatsAppEnum.Pendente;
    }

    // ── Domain Methods ────────────────────────────────────────────────────

    public void MarcarComoEnviada(string waMessageId)
    {
        WaMessageId = waMessageId;
        Status = StatusEnvioWhatsAppEnum.Enviado;
        EnviadaEm = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoEntregue()
    {
        Status = StatusEnvioWhatsAppEnum.Entregue;
        EntregueEm = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoLida()
    {
        Status = StatusEnvioWhatsAppEnum.Lido;
        LidaEm = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoFalhou(string descricaoErro)
    {
        Status = StatusEnvioWhatsAppEnum.Falhou;
        ErroEnvio = descricaoErro;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarcarComoRespondida()
    {
        Status = StatusEnvioWhatsAppEnum.Respondido;
        UpdatedAt = DateTime.UtcNow;
    }
}
