using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Obras.DTOs;

public record ObraDto(
    Guid Id, string Codigo, string Nome, string? Descricao,
    TipoObraEnum Tipo, TipoContratoObraEnum TipoContrato, StatusObraEnum Status,
    string? NomeCliente, string? ResponsavelTecnico,
    DateTime DataInicioPrevista, DateTime DataFimPrevista,
    DateTime? DataInicioReal, DateTime? DataFimReal,
    decimal ValorContrato, decimal ValorOrcado, decimal ValorRealizado,
    decimal PercentualConcluido,
    string Logradouro, string Numero, string? Complemento, string Bairro,
    string Cidade, string Estado, string Cep,
    string? Observacoes, string? FotoUrl,
    IEnumerable<FaseObraDto> Fases);
