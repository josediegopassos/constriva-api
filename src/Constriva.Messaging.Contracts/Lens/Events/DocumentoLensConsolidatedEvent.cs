using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Events;

/// <summary>
/// Evento publicado quando os dados extraídos de um documento são consolidados em uma compra.
/// </summary>
public record DocumentoLensConsolidatedEvent : IEvent
{
    /// <summary>Identificador único da mensagem.</summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>Identificador de correlação para rastreamento.</summary>
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();

    /// <summary>Data e hora de criação da mensagem.</summary>
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;

    /// <summary>Sistema de origem da mensagem.</summary>
    public string Origem { get; init; } = "Constriva.API";

    /// <summary>Identificador do processamento.</summary>
    public Guid ProcessamentoId { get; init; }

    /// <summary>Identificador da compra criada.</summary>
    public Guid CompraId { get; init; }

    /// <summary>Identificador do usuário que consolidou.</summary>
    public Guid UsuarioId { get; init; }

    /// <summary>Identificador da obra associada (opcional).</summary>
    public Guid? ObraId { get; init; }

    /// <summary>Identificador da empresa (tenant).</summary>
    public Guid EmpresaId { get; init; }

    /// <summary>Total de itens consolidados na compra.</summary>
    public int TotalItensConsolidados { get; init; }

    /// <summary>Total de itens rejeitados pelo usuário.</summary>
    public int TotalItensRejeitados { get; init; }

    /// <summary>Valor total dos itens consolidados.</summary>
    public decimal ValorTotal { get; init; }
}
