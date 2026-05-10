using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Commands;

public record ProcessDocumentoLensCommand : ICommand
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.API";
    public Guid ProcessamentoId { get; init; }
    public Guid UsuarioId { get; init; }
    public Guid EmpresaId { get; init; }
    public Guid? ObraId { get; init; }
    public Guid? CentroCustoId { get; init; }
    public string TipoDocumento { get; init; } = string.Empty;
    public string NomeArquivo { get; init; } = string.Empty;
    public string CaminhoArquivo { get; init; } = string.Empty;
    public string ExtensaoArquivo { get; init; } = string.Empty;
    public long TamanhoBytes { get; init; }
    public string? Observacoes { get; init; }
}
