namespace SkorinosGimnazija.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.API.Controllers.Base;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Menus.Dtos;
using System.Xml.Linq;
using SkorinosGimnazija.Application.Meta;
using Microsoft.AspNetCore.Authorization;
using SkorinosGimnazija.Application.Meta.Dtos;

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
