using System.Net;

namespace RO.DevTest.Domain.Exception;

public class InvalidLoginException : ApiException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    public InvalidLoginException() : base("Senha ou E-mail inválido") { }
}
