using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Compras;

public class PedidoCompra : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid? CotacaoId { get; set; }
    public Guid FornecedorId { get; set; }
    public Guid? AlmoxarifadoId { get; set; }
    public string Numero { get; set; } = null!;
    public StatusPedidoCompraEnum Status { get; set; } = StatusPedidoCompraEnum.Rascunho;
    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public DateTime? DataEntregaPrevista { get; set; }
    public DateTime? DataEntregaReal { get; set; }
    public FormaPagamentoEnum? FormaPagamento { get; set; }
    public string? CondicoesPagamento { get; set; }
    public string? LocalEntrega { get; set; }
    public string? Observacoes { get; set; }
    public decimal ValorFrete { get; set; }
    public decimal ValorDesconto { get; set; }
    public decimal ValorTotal { get; set; }
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public string? MotivoRejeicao { get; set; }
    public Guid CriadoPor { get; set; }

    public virtual Obras.Obra Obra { get; set; } = null!;
    public virtual Fornecedor Fornecedor { get; set; } = null!;
    public virtual ICollection<ItemPedidoCompra> Itens { get; set; } = new List<ItemPedidoCompra>();
    public virtual ICollection<RecebimentoCompra> Recebimentos { get; set; } = new List<RecebimentoCompra>();
}
