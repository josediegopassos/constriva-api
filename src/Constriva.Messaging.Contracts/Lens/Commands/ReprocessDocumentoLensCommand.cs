using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Commands;

/// <summary>
/// Comando para reprocessar um documento que teve erro ou precisa de nova análise.
/// </summary>
public record ReprocessDocumentoLensCommand : ICommand
{
    /// <summary>Identificador único da mensagem.</summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>Identificador de correlação para rastreamento.</summary>
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();

    /// <summary>Data e hora de criação da mensagem.</summary>
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;

    /// <summary>Sistema de origem da mensagem.</summary>
    public string Origem { get; init; } = "Constriva.API";

    /// <summary>Identificador do processamento original.</summary>
    public Guid ProcessamentoId { get; init; }

    /// <summary>Identificador do usuário que solicitou o reprocessamento.</summary>
    public Guid UsuarioId { get; init; }

    /// <summary>Motivo do reprocessamento.</summary>
    public string? MotivoReprocessamento { get; init; }
}
