using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Compras;

public class FornecedorCotacao : TenantEntity
{
    public Guid CotacaoId { get; set; }
    public Guid FornecedorId { get; set; }
    public StatusConviteCotacaoEnum Status { get; set; } = StatusConviteCotacaoEnum.Convidado;
    public DateTime ConvidadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? RespondeuEm { get; set; }

    public virtual Cotacao Cotacao { get; set; } = null!;
    public virtual Fornecedor Fornecedor { get; set; } = null!;
}
