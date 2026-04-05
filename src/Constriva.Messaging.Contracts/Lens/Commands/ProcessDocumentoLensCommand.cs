using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Commands;

/// <summary>
/// Comando para processar um documento via Constriva.Lens (OCR).
/// </summary>
public record ProcessDocumentoLensCommand : ICommand
{
    /// <summary>Identificador único da mensagem.</summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>Identificador de correlação para rastreamento.</summary>
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();

    /// <summary>Data e hora de criação da mensagem.</summary>
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;

    /// <summary>Sistema de origem da mensagem.</summary>
    public string Origem { get; init; } = "Constriva.API";

    /// <summary>Identificador do processamento no banco de dados.</summary>
    public Guid ProcessamentoId { get; init; }

    /// <summary>Identificador do usuário que solicitou o processamento.</summary>
    public Guid UsuarioId { get; init; }

    /// <summary>Identificador da empresa (tenant).</summary>
    public Guid EmpresaId { get; init; }

    /// <summary>Identificador da obra associada (opcional).</summary>
    public Guid? ObraId { get; init; }

    /// <summary>Identificador do centro de custo associado (opcional).</summary>
    public Guid? CentroCustoId { get; init; }

    /// <summary>Tipo do documento a ser processado.</summary>
    public string TipoDocumento { get; init; } = string.Empty;

    /// <summary>Nome original do arquivo enviado.</summary>
    public string NomeArquivo { get; init; } = string.Empty;

    /// <summary>Caminho completo do arquivo no servidor.</summary>
    public string CaminhoArquivo { get; init; } = string.Empty;

    /// <summary>Extensão do arquivo (.pdf, .jpg, etc.).</summary>
    public string ExtensaoArquivo { get; init; } = string.Empty;

    /// <summary>Tamanho do arquivo em bytes.</summary>
    public long TamanhoBytes { get; init; }

    /// <summary>Observações adicionais sobre o documento.</summary>
    public string? Observacoes { get; init; }
}
