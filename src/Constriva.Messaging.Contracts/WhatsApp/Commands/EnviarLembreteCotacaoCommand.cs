using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Commands;

public record EnviarLembreteCotacaoCommand : ICommand
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";
    public Guid CotacaoId { get; init; }
    public Guid FornecedorCotacaoId { get; init; }
    public Guid FornecedorId { get; init; }
    public Guid EmpresaId { get; init; }
    public string NumeroCotacao { get; init; } = string.Empty;
    public string TituloCotacao { get; init; } = string.Empty;
    public string NomeFornecedor { get; init; } = string.Empty;
    public string TelefoneWhatsApp { get; init; } = string.Empty;
    public DateTime DataLimiteResposta { get; init; }
    public int NumeroTentativa { get; init; }
    public DateTime EnvioOriginalEm { get; init; }
}
