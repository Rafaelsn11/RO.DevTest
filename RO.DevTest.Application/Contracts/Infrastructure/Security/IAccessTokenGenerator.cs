using System.Security.Claims;

namespace RO.DevTest.Application.Contracts.Infrastructure.Security;

/// <summary>
/// Interface para geração de tokens JWT
/// </summary>
public interface IAccessTokenGenerator
{
    /// <summary>
    /// Gera um token JWT para o identificador do usuário
    /// </summary>
    /// <param name="userIdentifier">Identificador do usuário</param>
    /// <returns>Token JWT</returns>
    public string Generate(string userIdentifier);

    /// <summary>
    /// Obtém a data de expiração do token
    /// </summary>
    /// <returns>Data de expiração do token</returns>
    public DateTime GetExpirationDate();
}
