using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record MedicaoContratoDto(
    Guid Id, Guid ContratoId, int Periodo, string Numero,
    decimal ValorMedicao, decimal ValorLiquido, decimal PercentualMedicao,
    StatusMedicaoEnum Status, DateTime DataInicio, DateTime DataFim,
    DateTime? DataSubmissao, DateTime? DataAprovacao);

public record MedicaoGeralDto(
    Guid Id, Guid ContratoId, string ContratoNumero, string? FornecedorNome,
    int Periodo, string Numero,
    decimal ValorMedicao, decimal ValorRetencao, decimal ValorLiquido, decimal PercentualMedicao,
    StatusMedicaoEnum Status, string StatusLabel,
    DateTime DataInicio, DateTime DataFim,
    DateTime? DataSubmissao, DateTime? DataAnalise, DateTime? DataAprovacao,
    string? MotivoRejeicao, string? Observacoes, DateTime CreatedAt);
