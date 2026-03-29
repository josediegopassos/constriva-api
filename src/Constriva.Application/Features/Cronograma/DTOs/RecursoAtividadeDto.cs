namespace Constriva.Application.Features.Cronograma.DTOs;

public record RecursoAtividadeDto(
    Guid Id, Guid AtividadeId, string TipoRecurso, string NomeRecurso,
    Guid? RecursoId, decimal Quantidade, string UnidadeMedida,
    decimal CustoUnitario, decimal CustoTotal
);
