using System.ComponentModel.DataAnnotations;
using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.DTOs;

public class InitProcessamentoLensDto
{
    [Required(ErrorMessage = "O tipo de documento é obrigatório.")]
    public TipoDocumentoLensEnum TipoDocumento { get; set; }

    public Guid? ObraId { get; set; }
    public Guid? CentroCustoId { get; set; }

    [MaxLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres.")]
    public string? Observacoes { get; set; }
}
