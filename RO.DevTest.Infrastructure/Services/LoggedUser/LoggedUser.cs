using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Contracts.Infrastructure.Security;
using RO.DevTest.Application.Contracts.Infrastructure.Services.LoggedUser;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentityAbstractor _identityAbstractor;

    public LoggedUser(ITokenProvider tokenProvider, IIdentityAbstractor identityAbstractor)
    {
        _tokenProvider = tokenProvider;
        _identityAbstractor = identityAbstractor;
    }

    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var user = await _identityAbstractor.FindUserByIdAsync(identifier);
        
        if (user == null)
            throw new UnauthorizedException("Usuário não encontrado");
            
        return user;
    }
}
