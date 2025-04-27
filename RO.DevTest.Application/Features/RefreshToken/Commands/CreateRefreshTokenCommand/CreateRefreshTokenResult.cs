namespace RO.DevTest.Application.Features.RefreshToken.Commands.CreateRefreshTokenCommand;

public class CreateRefreshTokenResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
