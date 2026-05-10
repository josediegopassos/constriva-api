using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Commands;

public record ReprocessDocumentoLensCommand : ICommand
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.API";
    public Guid ProcessamentoId { get; init; }
    public Guid UsuarioId { get; init; }
    public string? MotivoReprocessamento { get; init; }
}
