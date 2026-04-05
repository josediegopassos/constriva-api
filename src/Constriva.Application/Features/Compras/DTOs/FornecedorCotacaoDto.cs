using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record FornecedorCotacaoDto(
    Guid Id,
    Guid FornecedorId,
    string FornecedorNome,
    string? FornecedorDocumento,
    string? FornecedorEmail,
    StatusConviteCotacaoEnum Status,
    DateTime ConvidadoEm,
    DateTime? RespondeuEm);

public record ConvidarFornecedoresDto(
    List<Guid> FornecedorIds);
