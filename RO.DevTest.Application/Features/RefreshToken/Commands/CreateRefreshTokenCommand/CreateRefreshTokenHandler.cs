using MediatR;
using RO.DevTest.Application.Contracts.Infrastructure.Security;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;
using RO.DevTest.Domain.ValueObjects;

namespace RO.DevTest.Application.Features.RefreshToken.Commands.CreateRefreshTokenCommand;

public class CreateRefreshTokenHandler : IRequestHandler<CreateRefreshTokenCommand, CreateRefreshTokenResult>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public CreateRefreshTokenHandler(
        ITokenRepository tokenRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _tokenRepository = tokenRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<CreateRefreshTokenResult> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
         var refreshToken = await _tokenRepository.Get(request.RefreshToken);

        if(refreshToken is null)
            throw new RefreshTokenNotFoundException();

        var refreshTokenValidUntil = refreshToken.CreatedOn.AddDays(RuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS);
        if (DateTime.Compare(refreshTokenValidUntil, DateTime.UtcNow) < 0)
            throw new RefreshTokenExpiredException();

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _tokenRepository.SaveNewRefreshToken(newRefreshToken, cancellationToken);

        return new CreateRefreshTokenResult
        {
            AccessToken = _accessTokenGenerator.Generate(refreshToken.User.Id),
            RefreshToken = newRefreshToken.Value
        };
    }
}
