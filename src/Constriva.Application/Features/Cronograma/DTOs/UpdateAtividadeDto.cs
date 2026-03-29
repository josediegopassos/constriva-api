namespace Constriva.Application.Features.Cronograma.DTOs;

public record UpdateAtividadeDto(
    string Nome,
    string? Descricao,
    int Ordem,
    int Nivel,
    int DuracaoDias,
    DateTime DataInicio,
    DateTime DataFim,
    DateTime? DataInicioReal,
    DateTime? DataFimReal,
    DateTime? DataInicioReprogramada,
    DateTime? DataFimReprogramada,
    decimal PercentualConcluido,
    Guid? AtividadePaiId = null,
    Guid? FaseObraId = null,
    bool EAgrupadora = false,
    bool EMarcador = false,
    decimal CustoOrcado = 0,
    decimal CustoRealizado = 0,
    string? ResponsavelId = null,
    string? Cor = null,
    string? Observacoes = null);
