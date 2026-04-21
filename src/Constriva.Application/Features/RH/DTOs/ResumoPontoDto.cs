namespace Constriva.Application.Features.RH.DTOs;

public record ResumoPontoDto(
    Guid FuncionarioId, string FuncionarioNome,
    int DiasUteis, int DiasPresente, int Faltas,
    decimal HorasNormais, decimal HorasExtras, decimal HorasTotais,
    int TotalRegistros, int PendentesAprovacao, int Aprovados, int Reprovados);
