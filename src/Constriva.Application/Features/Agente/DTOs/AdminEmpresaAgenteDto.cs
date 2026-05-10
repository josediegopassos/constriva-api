namespace Constriva.Application.Features.Agente.DTOs;

public record AdminEmpresaAgenteDto(
    Guid EmpresaId, string EmpresaNome, string TierNome,
    long TokensUtilizados, long TokensLimite, bool Ativo);
