namespace Constriva.Application.Features.Cronograma.DTOs;

public record CreateAtividadeDto(
    string Nome,
    string? Descricao,
    int Ordem,
    DateTime DataInicioPrevista,
    DateTime DataFimPrevista,
    decimal DuracaoDias,
    Guid? AtividadePaiId = null,
    Guid? FaseObraId = null,
    int Nivel = 1,
    bool EAgrupadora = false,
    bool EMarcador = false,
    decimal CustoOrcado = 0,
    string? ResponsavelId = null,
    string? Cor = null,
    string? Observacoes = null,
    IEnumerable<Guid>? Predecessoras = null);
