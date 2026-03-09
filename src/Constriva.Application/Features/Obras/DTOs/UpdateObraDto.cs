using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Obras.DTOs;

public record UpdateObraDto(
    string Nome, TipoObraEnum Tipo,
    string? NomeCliente, string? ResponsavelTecnico, string? Descricao,
    string? Observacoes,
    string? Logradouro, string? Numero, string? Complemento, string? Bairro,
    DateTime? DataInicioPrevista, DateTime? DataFimPrevista,
    decimal? ValorContrato, string? FotoUrl);
