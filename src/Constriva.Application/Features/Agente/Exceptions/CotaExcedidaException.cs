namespace Constriva.Application.Features.Agente.Exceptions;

public class CotaExcedidaException : Exception
{
    public decimal PercentualUsado { get; }
    public long TokensRestantes { get; }

    public CotaExcedidaException(string message, decimal percentualUsado = 100, long tokensRestantes = 0)
        : base(message)
    {
        PercentualUsado = percentualUsado;
        TokensRestantes = tokensRestantes;
    }
}
