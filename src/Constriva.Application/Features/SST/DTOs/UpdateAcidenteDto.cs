namespace Constriva.Application.Features.SST.DTOs;

public record UpdateAcidenteDto(string Descricao, DateTime DataAcidente, bool AfastamentoMedico, int? DiasAfastamento, string? CausaRaiz, string? AcoesCorretivas);
