using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class DadosBancarios : TenantEntity
{
    public int? BancoId { get; set; }
    public string? Agencia { get; set; }
    public string? Conta { get; set; }
    public string? PixChave { get; set; }
    public virtual Banco? Banco { get; set; }
}
