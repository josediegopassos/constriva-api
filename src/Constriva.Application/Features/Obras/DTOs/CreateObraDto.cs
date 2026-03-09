using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Obras.DTOs;

public record CreateObraDto(
    string Nome, TipoObraEnum Tipo, TipoContratoObraEnum TipoContrato,
    string? NomeCliente, string? ResponsavelTecnico,
    DateTime DataInicioPrevista, DateTime DataFimPrevista,
    decimal ValorContrato,
    string Logradouro, string Numero, string? Complemento, string Bairro,
    string Cidade, string Estado, string Cep,
    string? Descricao = null, string? Observacoes = null,
    string? CreaResponsavel = null,
    string? NumeroART = null,
    string? NumeroRRT = null,
    string? NumeroAlvara = null,
    DateTime? ValidadeAlvara = null,
    double? AreaTotal = null,
    double? AreaConstruida = null,
    int? NumeroAndares = null,
    int? NumeroUnidades = null,
    decimal ValorOrcado = 0,
    double? Latitude = null,
    double? Longitude = null);
