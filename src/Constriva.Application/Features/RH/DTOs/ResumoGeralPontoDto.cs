namespace Constriva.Application.Features.RH.DTOs;

public record ResumoGeralPontoDto(
    int TotalFuncionarios, int DiasUteis,
    decimal HorasNormais, decimal HorasExtras, decimal HorasTotais,
    int TotalFaltas, int TotalRegistros,
    int PendentesAprovacao, int Aprovados, int Reprovados,
    IEnumerable<ResumoPontoDto> PorFuncionario);
