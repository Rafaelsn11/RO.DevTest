using System.Net;

namespace RO.DevTest.Domain.Exception;

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message) : base(message) {}

    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
