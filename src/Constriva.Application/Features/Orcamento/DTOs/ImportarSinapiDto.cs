namespace Constriva.Application.Features.Orcamento.DTOs;

public record ImportarSinapiDto(
    Guid? GrupoId,
    List<ItemSinapiDto>? Itens);
