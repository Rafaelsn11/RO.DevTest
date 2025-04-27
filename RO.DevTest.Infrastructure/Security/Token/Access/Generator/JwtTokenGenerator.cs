using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RO.DevTest.Application.Contracts.Infrastructure.Security;
using RO.DevTest.Application.Contracts.Infrastructure;

namespace RO.DevTest.Infrastructure.Security.Token.Access.Generator;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey, IIdentityAbstractor identityAbstractor) : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;
    private readonly IIdentityAbstractor _identityAbstractor = identityAbstractor;   
    public string Generate(string userIdentifier)
    {
        var user = _identityAbstractor.FindUserByIdAsync(userIdentifier).Result;
        var roles = _identityAbstractor.GetUserRolesAsync(user!).Result;

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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