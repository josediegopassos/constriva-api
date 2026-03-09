using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record MaterialDto(Guid Id, string Codigo, string Nome, string UnidadeMedida, TipoInsumoEnum Tipo, string? CodigoSINAPI, string? Marca);
