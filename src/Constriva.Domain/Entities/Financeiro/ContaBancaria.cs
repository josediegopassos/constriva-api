using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Financeiro;

public class ContaBancaria : TenantEntity
{
    public string Nome { get; set; } = null!;
    public string BancoNome { get; set; } = null!;
    public string? BancoCodigo { get; set; }
    public string Agencia { get; set; } = null!;
    public string Conta { get; set; } = null!;
    public string TipoConta { get; set; } = null!;  // Corrente, Poupanca, Investimento
    public decimal SaldoInicial { get; set; }
    public decimal SaldoAtual { get; set; }
    public bool Ativo { get; set; } = true;
    public string? PixChave { get; set; }
    public virtual ICollection<LancamentoFinanceiro> Lancamentos { get; set; } = new List<LancamentoFinanceiro>();
    public virtual ICollection<Transferencia> TransferenciasOrigem { get; set; } = new List<Transferencia>();
    public virtual ICollection<Transferencia> TransferenciasDestino { get; set; } = new List<Transferencia>();
}
