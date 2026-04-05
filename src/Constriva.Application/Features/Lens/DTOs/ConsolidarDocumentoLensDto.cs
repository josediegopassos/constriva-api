using System.ComponentModel.DataAnnotations;

namespace Constriva.Application.Features.Lens.DTOs;

public class ConsolidarDocumentoLensDto
{
    [Required(ErrorMessage = "A obra é obrigatória para consolidação.")]
    public Guid ObraId { get; set; }

    public Guid? CentroCustoId { get; set; }

    [Required(ErrorMessage = "O fornecedor é obrigatório para consolidação.")]
    public Guid FornecedorId { get; set; }

    [MaxLength(1000, ErrorMessage = "Observações devem ter no máximo 1000 caracteres.")]
    public string? Observacoes { get; set; }
}
