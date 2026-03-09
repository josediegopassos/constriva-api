using Constriva.Domain.Enums;

namespace Constriva.Application.Features.SST.DTOs;

public record CreateAcidenteDto(
    Guid ObraId, TipoAcidenteEnum Tipo, string NomeFuncionario, string Local,
    string DescricaoAcidente, bool AfastamentoMedico, int? DiasAfastamento,
    DateTime DataHoraAcidente, Guid? FuncionarioId = null);
