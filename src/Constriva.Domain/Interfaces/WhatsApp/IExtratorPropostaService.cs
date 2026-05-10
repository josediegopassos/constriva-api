using Constriva.Domain.ValueObjects.WhatsApp;

namespace Constriva.Domain.Interfaces.WhatsApp;

public interface IExtratorPropostaService
{
    Task<PropostaExtraidaValueObject> ExtrairAsync(
        EntradaExtratorValueObject entrada,
        CancellationToken cancellationToken = default);
}
