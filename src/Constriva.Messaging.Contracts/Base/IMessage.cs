namespace Constriva.Messaging.Contracts.Base;

public interface IMessage
{
    Guid Id { get; }
    Guid CorrelacaoId { get; }
    DateTime CriadoEm { get; }
    string Origem { get; }
}
