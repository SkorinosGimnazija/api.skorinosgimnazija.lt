namespace SkorinosGimnazija.API.Controllers;

using Application.Meta;
using Application.Meta.Dtos;
using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public sealed class MetaController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("menus", Name = "GetMenusMeta")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<MenuMetaDto>> GetMenus(CancellationToken ct)
    {
        return await Mediator.Send(new MenuMetaList.Query(), ct);
    }

    [AllowAnonymous]
    [HttpGet("posts", Name = "GetPostsMeta")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<PostMetaDto>> GetPosts(CancellationToken ct)
    {
        return await Mediator.Send(new PostMetaList.Query(), ct);
    }
}