using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Cronograma.DTOs;

public record AtividadeDetalhadaDto(
    Guid Id, string Codigo, string Nome, string? Descricao,
    int Nivel, int Ordem, bool EAgrupadora, bool EMarcador,
    StatusAtividadeEnum Status, int DuracaoDias,
    decimal PercentualConcluido,
    DateTime DataInicioPlanejada, DateTime DataFimPlanejada,
    DateTime? DataInicioReal, DateTime? DataFimReal,
    DateTime? DataInicioReprogramada, DateTime? DataFimReprogramada,
    decimal BCWS, decimal BCWP, decimal ACWP,
    decimal CustoOrcado, decimal CustoRealizado,
    bool NoCaminhoCritico, int Folga,
    string? ResponsavelId, string? Cor, string? Observacoes,
    Guid? AtividadePaiId,
    IEnumerable<VinculoAtividadeDto> Predecessoras,
    IEnumerable<VinculoAtividadeDto> Sucessoras,
    IEnumerable<RecursoAtividadeDto> Recursos,
    IEnumerable<ProgressoAtividadeDto> Progressos
);
