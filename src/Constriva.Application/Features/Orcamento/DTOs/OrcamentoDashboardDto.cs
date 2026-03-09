namespace Constriva.Application.Features.Orcamento.DTOs;

public record OrcamentoDashboardDto(
    int TotalOrcamentos,
    int Rascunhos,
    int EmRevisao,
    int Aprovados,
    decimal ValorTotalAprovados,
    decimal ValorMedioOrcamentos,
    OrcamentoResumoDto? UltimoAtualizado);
