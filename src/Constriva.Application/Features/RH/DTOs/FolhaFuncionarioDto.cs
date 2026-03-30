namespace Constriva.Application.Features.RH.DTOs;

public record FolhaFuncionarioDto(
    Guid Id, Guid FolhaId, string Competencia,
    Guid FuncionarioId, string FuncionarioNome,
    decimal SalarioBruto, decimal HorasExtras, decimal ValorHorasExtras,
    decimal AdicionalNoturno, decimal Periculosidade, decimal Insalubridade,
    decimal OutrasVerbas, decimal TotalProventos,
    decimal INSS, decimal IRRF, decimal ValeTransporte, decimal ValeRefeicao,
    decimal OutrosDescontos, decimal TotalDescontos,
    decimal SalarioLiquido, decimal FGTS);
