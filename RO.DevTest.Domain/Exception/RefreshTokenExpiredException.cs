using System.Net;

namespace RO.DevTest.Domain.Exception;

public class RefreshTokenExpiredException : ApiException
{
    public RefreshTokenExpiredException() : base("Sessão inválida"){}

    public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
}
