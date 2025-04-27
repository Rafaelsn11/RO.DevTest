using System.Net;

namespace RO.DevTest.Domain.Exception;

public class RefreshTokenNotFoundException : ApiException
{
    public RefreshTokenNotFoundException() : base("Sessão expirada"){}

    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
