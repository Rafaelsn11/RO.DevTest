using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using RO.DevTest.Application.Features.RefreshToken.Commands.CreateRefreshTokenCommand;

namespace RO.DevTest.WebApi.Controllers;

[Route("api/token")]
[OpenApiTags("Token")]
public class TokenController(IMediator _mediator) : ControllerBase
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(CreateRefreshTokenResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken(CreateRefreshTokenCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
