namespace Constriva.Messaging.Contracts.Base;

/// <summary>
/// Interface base para todas as mensagens do sistema de mensageria.
/// </summary>
public interface IMessage
{
    /// <summary>Identificador único da mensagem.</summary>
    Guid Id { get; }

    /// <summary>Identificador de correlação para rastreamento entre mensagens.</summary>
    Guid CorrelacaoId { get; }

    /// <summary>Data e hora de criação da mensagem.</summary>
    DateTime CriadoEm { get; }

    /// <summary>Sistema de origem da mensagem.</summary>
    string Origem { get; }
}
