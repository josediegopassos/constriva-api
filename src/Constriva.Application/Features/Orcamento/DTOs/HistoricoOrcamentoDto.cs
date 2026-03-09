namespace Constriva.Application.Features.Orcamento.DTOs;

public record HistoricoOrcamentoDto(
    Guid Id,
    string Descricao,
    decimal ValorAnterior,
    decimal ValorNovo,
    Guid UsuarioId,
    DateTime CreatedAt);
