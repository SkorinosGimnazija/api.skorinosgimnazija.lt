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

    [Route("admin/menus")]
    [Authorize(Roles = Roles.Admin)]
    public class MenusController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Menu>>> GetMenus(CancellationToken ct)
        {
            return await Mediator.Send(new MenuList.Query(), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Menu>> GetMenu(int id, CancellationToken ct)
        {
            return await Mediator.Send(new MenuDetails.Query(id), ct);
        }

        [HttpPost]
        public async Task<ActionResult<Menu>> CreateMenu(MenuCreateDto menu, CancellationToken ct)
        {
            return await Mediator.Send(new MenuCreate.Command(menu), ct);
        }

        [HttpPut]
        public async Task<IActionResult> EditMenu(MenuEditDto menu, CancellationToken ct)
        {
            return await Mediator.Send(new MenuEdit.Command(menu), ct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenu(int id, CancellationToken ct)
        {
            return await Mediator.Send(new MenuDelete.Command(id), ct);
        }
    }
}