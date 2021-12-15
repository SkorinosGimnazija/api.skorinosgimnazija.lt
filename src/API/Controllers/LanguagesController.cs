namespace SkorinosGimnazija.API.Controllers;

using Application.Languages;
using Application.Languages.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public class LanguagesController : BaseApiController
{ 
    [AllowAnonymous]
    [HttpGet("public", Name = "GetPublicLanguages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<LanguageDto>> GetAll(CancellationToken ct)
    {
        return await Mediator.Send(new PublicLanguageList.Query(), ct);
    }
}