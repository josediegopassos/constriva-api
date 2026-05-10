namespace Constriva.Messaging.Services.Lens;

public interface ILensAuthenticationService
{
    Task<string> ObterTokenAsync(CancellationToken ct);
    Task InvalidarTokenAsync();
}
