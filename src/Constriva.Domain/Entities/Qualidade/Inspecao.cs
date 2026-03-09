using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Qualidade;

public class Inspecao : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? FaseObraId { get; set; }
    public Guid? ChecklistModeloId { get; set; }
    public string Numero { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public StatusInspecaoEnum Status { get; set; } = StatusInspecaoEnum.Pendente;
    public DateTime DataProgramada { get; set; }
    public DateTime? DataRealizacao { get; set; }
    public string? Localicacao { get; set; }  // Bloco, Apartamento, Pavimento
    public string? ResponsavelInsId { get; set; }
    public Guid? InspetorId { get; set; }
    public string? Resultado { get; set; }
    public string? Observacoes { get; set; }
    public bool TemNaoConformidade { get; set; } = false;

    public virtual ICollection<ItemInspecao> Itens { get; set; } = new List<ItemInspecao>();
    public virtual ICollection<FotoInspecao> Fotos { get; set; } = new List<FotoInspecao>();
    public virtual ICollection<NaoConformidade> NaoConformidades { get; set; } = new List<NaoConformidade>();
}
