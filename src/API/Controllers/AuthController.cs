namespace SkorinosGimnazija.API.Controllers;

using Application.Authorization;
using Application.Authorization.Dtos;
using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public sealed class AuthController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("authorize", Name = "Authorize")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserAuthDto>> Authorize(GoogleAuthDto googleAuth)
    {
        return await Mediator.Send(new UserAuthorize.Command(googleAuth));
    }
}