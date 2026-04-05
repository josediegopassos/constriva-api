namespace Constriva.Messaging.Services.Lens;

/// <summary>
/// Serviço de autenticação com o Constriva.Lens via OAuth2 client credentials.
/// </summary>
public interface ILensAuthenticationService
{
    /// <summary>
    /// Obtém um token de acesso válido para o Constriva.Lens.
    /// </summary>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Token de acesso.</returns>
    Task<string> ObterTokenAsync(CancellationToken ct);

    /// <summary>
    /// Invalida o token em cache, forçando obtenção de um novo na próxima chamada.
    /// </summary>
    Task InvalidarTokenAsync();
}
