using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RO.DevTest.Application.Contracts.Infrastructure.Security;

namespace RO.DevTest.Infrastructure.Security.Token.Access.Generator;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;

    public string Generate(string userIdentifier)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public DateTime GetExpirationDate() => DateTime.UtcNow.AddMinutes(_expirationTimeMinutes);
}