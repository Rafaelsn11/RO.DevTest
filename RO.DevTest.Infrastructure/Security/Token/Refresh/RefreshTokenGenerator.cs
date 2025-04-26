using RO.DevTest.Application.Contracts.Infrastructure.Security;

namespace RO.DevTest.Infrastructure.Security.Token.Refresh;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
