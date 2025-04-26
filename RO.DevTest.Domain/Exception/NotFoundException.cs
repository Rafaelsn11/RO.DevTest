using System.Net;

namespace RO.DevTest.Domain.Exception;

public class NotFoundException : ApiException
{
    public NotFoundException(string message) : base(message) {}
    
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
