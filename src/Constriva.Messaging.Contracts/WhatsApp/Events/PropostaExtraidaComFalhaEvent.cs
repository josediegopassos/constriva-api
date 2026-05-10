using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Events;

public record PropostaExtraidaComFalhaEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";

    public Guid CotacaoId { get; init; }
    public Guid FornecedorCotacaoId { get; init; }
    public Guid FornecedorId { get; init; }
    public Guid EmpresaId { get; init; }
    public string WaMessageId { get; init; } = string.Empty;
    public DateTime FalhouEm { get; init; }
    public MotivoFalhaExtracao Motivo { get; init; }
    public string DescricaoFalha { get; init; } = string.Empty;
    public string? MensagemParaGestor { get; init; }
    public int? NivelConfiancaObtido { get; init; }
    public bool RequerIntervencaoManual { get; init; }
}

public enum MotivoFalhaExtracao
{
    ConfiancaInsuficiente = 0,
    RespostaNaoEProposta = 1,
    ErroApiOpenAI = 2,
    MidiaInacessivel = 3,
    FormatoNaoSuportado = 4,
    Desconhecido = 99
}
