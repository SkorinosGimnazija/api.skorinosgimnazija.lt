namespace API.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Menus;
    using Application.Menus.Dtos;
    using Application.Posts;
    using Application.Posts2;
    using Domain.Auth;
    using Domain.CMS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("menus")]
    public class PublicMenusController : BaseApiController
    {
        [HttpGet("{domain}/{language}")]
        public async Task<ActionResult<List<PublicMenuDto>>> GetMenus(string domain, string language, CancellationToken ct)
        {
            return await Mediator.Send(new PublicMenuList.Query(domain, language), ct);
        }
    }
}