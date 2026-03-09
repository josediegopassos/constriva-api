namespace Constriva.Application.Features.Empresas.DTOs;

public record ModulosConfigDto(
    bool Obras, bool Estoque, bool Cronograma, bool Orcamento, bool Compras,
    bool Qualidade, bool Contratos, bool RH, bool Financeiro, bool SST, bool GED, bool Relatorios
);
