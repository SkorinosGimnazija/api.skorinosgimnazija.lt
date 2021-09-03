namespace API.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
using Application.Domains;
    using Application.Menus;
    using Application.Menus.Dtos;
    using Application.Posts;
    using Application.Posts2;
    using Domain.Auth;
    using Domain.CMS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("menus")]
    [Authorize(Roles = Roles.Admin)]
    public class MenusController : BaseApiController
    {
        [HttpGet]
        public async Task<List<MenuDto>> GetMenus(CancellationToken ct)
        {
            return await Mediator.Send(new MenuList.Query(), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuDto>> GetMenu(int id, CancellationToken ct)
        {
            var entity = await Mediator.Send(new MenuDetails.Query(id), ct);
            if (entity is null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<MenuDto>> CreateMenu(MenuCreateDto menu, CancellationToken ct)
        {
            var entity = await Mediator.Send(new MenuCreate.Command(menu), ct);

            return CreatedAtAction(nameof(GetMenu), new { entity.Id }, entity);
        }

        [HttpPut]
        public async Task<IActionResult> EditMenu(MenuEditDto menu, CancellationToken ct)
        {
            var result = await Mediator.Send(new MenuEdit.Command(menu), ct);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenu(int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new MenuDelete.Command(id), ct);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("public/{language}")]
        public async Task<List<MenuDto>> GetMenus(string language, CancellationToken ct)
        {
            return await Mediator.Send(new PublicMenuList.Query(language), ct);
        }
    }
}