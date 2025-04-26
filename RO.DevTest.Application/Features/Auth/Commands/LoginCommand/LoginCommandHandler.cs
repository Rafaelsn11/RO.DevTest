using MediatR;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Contracts.Infrastructure.Security;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;
namespace RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse> 
{
    private readonly IIdentityAbstractor _identityAbstractor;
    private readonly ITokenRepository _tokenRepository;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginCommandHandler(
        IIdentityAbstractor identityAbstractor, ITokenRepository tokenRepository, IRefreshTokenGenerator refreshTokenGenerator,
        IAccessTokenGenerator accessTokenGenerator
        )
    {
        _identityAbstractor = identityAbstractor;
        _tokenRepository = tokenRepository;
        _refreshTokenGenerator = refreshTokenGenerator;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken) 
    {
        var user = await _identityAbstractor.FindUserByEmailAsync(request.Email);

        if (user is null)
            throw new InvalidLoginException();

        var result = await _identityAbstractor.PasswordSignInAsync(user, request.Password);

        if (!result.Succeeded)
            throw new InvalidLoginException();
        
        var accessToken = _accessTokenGenerator.Generate(user.Id);

        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpirationDate = _accessTokenGenerator.GetExpirationDate()
        };
   
    }

    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = new RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = user.Id
        };

        await _tokenRepository.SaveNewRefreshToken(refreshToken);

        return refreshToken.Value;
    }
}
