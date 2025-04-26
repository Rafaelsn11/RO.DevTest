namespace RO.DevTest.Application.Contracts.Infrastructure.Security;

/// <summary>
/// Provides functionality for validating JWT tokens and extracting user identifiers.
/// This interface is specifically designed to validate access tokens and retrieve
/// the user's unique identifier (GUID) from the token's claims.
/// </summary>
public interface IAccessTokenValidator
{
    /// <summary>
    /// Validates a JWT token and extracts the user's unique identifier from it.
    /// The method performs token validation and retrieves the user's GUID from
    /// the token's claims (specifically from the 'sid' claim).
    /// </summary>
    /// <param name="token">The JWT token string to validate</param>
    /// <returns>The user's unique identifier (GUID) extracted from the token</returns>
    /// <exception cref="SecurityTokenException">Thrown when the token is invalid or expired</exception>
    /// <exception cref="InvalidOperationException">Thrown when the user identifier claim is not found or invalid</exception>
    public Guid ValidateAndGetUserIdentifier(string token);
} 