namespace API.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Menus;
    using Application.Menus.Dtos;
    using Domain.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Roles.Admin)]
    public class MenuController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{domain}/{language}")]
        public async Task<ActionResult<List<MenuDto>>> GetMenus(string domain, string language, CancellationToken ct)
        {
            return await Mediator.Send(new PublicList.Query(domain, language), ct);
        }
    }
}