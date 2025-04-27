using MediatR;

namespace RO.DevTest.Application.Features.RefreshToken.Commands.CreateRefreshTokenCommand;

public class CreateRefreshTokenCommand : IRequest<CreateRefreshTokenResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}
