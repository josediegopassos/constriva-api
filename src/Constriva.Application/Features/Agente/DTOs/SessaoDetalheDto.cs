namespace Constriva.Application.Features.Agente.DTOs;

public record SessaoDetalheDto(Guid Id, DateTime AtualizadaEm, IEnumerable<MensagemDto> Mensagens);
