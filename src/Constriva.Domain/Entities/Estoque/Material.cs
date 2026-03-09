using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Estoque;

public class Material : TenantEntity
{
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? Especificacao { get; set; }
    public string UnidadeMedida { get; set; } = null!;
    public string? CodigoBarras { get; set; }
    public string? CodigoSINAPI { get; set; }
    public string? Marca { get; set; }
    public string? Fabricante { get; set; }
    public TipoInsumoEnum Tipo { get; set; } = TipoInsumoEnum.Material;
    public Guid? GrupoId { get; set; }
    public decimal EstoqueMinimo { get; set; } = 0;
    public decimal EstoqueMaximo { get; set; } = 0;
    public decimal PrecoCustoMedio { get; set; } = 0;
    public decimal PrecoUltimaCompra { get; set; } = 0;
    public bool Ativo { get; set; } = true;
    public string? ImagemUrl { get; set; }
    public string? Observacoes { get; set; }
    public bool ControlaLote { get; set; } = false;
    public bool ControlaValidade { get; set; } = false;

    public virtual GrupoMaterial? Grupo { get; set; }
    public virtual ICollection<EstoqueSaldo> Saldos { get; set; } = new List<EstoqueSaldo>();
    public virtual ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
}
