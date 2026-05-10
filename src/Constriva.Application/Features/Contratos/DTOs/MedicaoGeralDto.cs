using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record MedicaoGeralDto(
    Guid Id, Guid ContratoId, string ContratoNumero, string? FornecedorNome,
    int Periodo, string Numero,
    decimal ValorMedicao, decimal ValorRetencao, decimal ValorLiquido, decimal PercentualMedicao,
    StatusMedicaoEnum Status, string StatusLabel,
    DateTime DataInicio, DateTime DataFim,
    DateTime? DataSubmissao, DateTime? DataAnalise, DateTime? DataAprovacao,
    string? MotivoRejeicao, string? Observacoes, DateTime CreatedAt);
