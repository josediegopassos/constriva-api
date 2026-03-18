namespace Constriva.Application.Features.Auth.DTOs;

public record ModulosEmpresaDto(
    bool Obras, bool Estoque, bool Cronograma, bool Orcamento, bool Compras,
    bool Qualidade, bool Contratos, bool RH, bool Financeiro, bool SST, bool GED, bool Relatorios, bool Clientes
);
