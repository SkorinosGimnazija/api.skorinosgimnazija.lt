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

    [Authorize(Roles = Roles.Admin)]
    public class MenuController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{domain}/{language}")]
        public async Task<ActionResult<List<PublicMenuDto>>> GetMenus(string domain, string language, CancellationToken ct)
        {
            return await Mediator.Send(new PublicMenuList.Query(domain, language), ct);
        }
  
        [HttpGet("admin")]
        public async Task<ActionResult<List<Menu>>> GetAdminMenus(CancellationToken ct)
        {
            return await Mediator.Send(new MenuList.Query(), ct);
        }

        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<Menu>> GetMenu(int id, CancellationToken ct)
        {
            return await Mediator.Send(new MenuDetails.Query(id), ct);
        }

        [HttpPost("admin")]
        public async Task<ActionResult<Menu>> CreateMenu(MenuCreateDto menu, CancellationToken ct)
        {
            return await Mediator.Send(new MenuCreate.Command(menu), ct);
        }

        [HttpPut("admin")]
        public async Task<IActionResult> EditMenu(MenuEditDto menu, CancellationToken ct)
        {
            return await Mediator.Send(new MenuEdit.Command(menu), ct);
        }

        [HttpDelete("admin/{id:int}")]
        public async Task<IActionResult> DeleteMenu(int id, CancellationToken ct)
        {
            return await Mediator.Send(new MenuDelete.Command(id), ct);
        }
    }
}