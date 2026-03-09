using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.RH;

public class RegistroPonto : TenantEntity
{
    public Guid FuncionarioId { get; set; }
    public Guid? ObraId { get; set; }
    public DateTime DataHora { get; set; }
    public TipoRegistroPontoEnum Tipo { get; set; }
    public string? HorarioPrevisto { get; set; }
    public decimal? HorasExtras { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Dispositivo { get; set; }
    public bool Online { get; set; } = true;
    public bool Manual { get; set; } = false;
    public string? Justificativa { get; set; }
    public Guid? AprovadoPor { get; set; }
    public virtual Funcionario Funcionario { get; set; } = null!;
}
