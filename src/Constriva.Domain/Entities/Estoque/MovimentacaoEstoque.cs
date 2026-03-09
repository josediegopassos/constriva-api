using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Estoque;

public class MovimentacaoEstoque : TenantEntity
{
    public Guid AlmoxarifadoId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid? ObraId { get; set; }
    public Guid? FaseObraId { get; set; }
    public Guid? AlmoxarifadoDestinoId { get; set; }
    public TipoMovimentacaoEstoqueEnum Tipo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal ValorTotal => Quantidade * PrecoUnitario;
    public decimal SaldoAnterior { get; set; }
    public decimal SaldoPosterior { get; set; }
    public DateTime DataMovimentacao { get; set; } = DateTime.UtcNow;
    public string? NumeroDocumento { get; set; }
    public string? NumeroNF { get; set; }
    public string? Lote { get; set; }
    public DateTime? Validade { get; set; }
    public string? Observacao { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid? RequisicaoId { get; set; }
    public Guid? PedidoCompraId { get; set; }

    public virtual Almoxarifado Almoxarifado { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;
}
