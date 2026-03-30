namespace Constriva.Application.Features.RH.DTOs;

public record DadosBancariosDto(
    int? BancoId,
    string? BancoNome,
    string? Agencia,
    string? Conta,
    string? PixChave);
