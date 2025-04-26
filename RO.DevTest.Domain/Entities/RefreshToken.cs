using RO.DevTest.Domain.Abstract;

namespace RO.DevTest.Domain.Entities;

/// <summary>
/// Represents a refresh token used for maintaining user sessions without requiring frequent re-authentication.
/// This token is used to obtain new access tokens when the current one expires.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// The actual refresh token value used for authentication.
    /// </summary>
    public required string Value { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the user associated with this refresh token.
    /// </summary>
    public required string UserId { get; set; }

    /// <summary>
    /// Navigation property for the associated user.
    /// </summary>
    public User User { get; set; } = default!;
}
