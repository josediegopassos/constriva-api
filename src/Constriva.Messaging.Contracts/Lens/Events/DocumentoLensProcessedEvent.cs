using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Events;

/// <summary>
/// Evento publicado quando o status de um processamento Lens é atualizado.
/// </summary>
public record DocumentoLensProcessedEvent : IEvent
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

    /// <summary>Status atual do processamento.</summary>
    public string Status { get; init; } = string.Empty;

    /// <summary>Mensagem descritiva do status.</summary>
    public string Mensagem { get; init; } = string.Empty;
}
