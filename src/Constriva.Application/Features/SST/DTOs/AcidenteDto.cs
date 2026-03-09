using Constriva.Domain.Enums;

namespace Constriva.Application.Features.SST.DTOs;

public record AcidenteDto(
    Guid Id, Guid ObraId, TipoAcidenteEnum Tipo, string NomeFuncionario,
    string DescricaoAcidente, string Local,
    bool AfastamentoMedico, int? DiasAfastamento,
    DateTime DataHoraAcidente, string? NumeroCAT, DateTime CreatedAt);
