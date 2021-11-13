namespace API.Controllers;

using Application.Menus;
using Application.Menus.Dtos;
using Base;
using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public class MenusController : BaseApiController
{
    [HttpGet(Name = "GetMenus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<MenuDto>> GetMenus(CancellationToken ct)
    {
        return await Mediator.Send(new MenuList.Query(), ct);
    }

    [HttpGet("{id:int}", Name = "GetMenuById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetMenu(int id, CancellationToken ct)
    {
        var entity = await Mediator.Send(new MenuDetails.Query(id), ct);
        if (entity is null)
        {
            return NotFound();
        }

        return entity;
    }

    [HttpPost(Name = "CreateMenu")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MenuDto>> CreateMenu(MenuCreateDto menu, CancellationToken ct)
    {
        var entity = await Mediator.Send(new MenuCreate.Command(menu), ct);

        return CreatedAtAction(nameof(GetMenu), new { entity.Id }, entity);
    }

    [HttpPut(Name = "EditMenu")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditMenu(MenuEditDto menu, CancellationToken ct)
    {
        var result = await Mediator.Send(new MenuEdit.Command(menu), ct);
        if (!result)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteMenu")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [HttpGet("public/{language}", Name = "GetPublicMenusByLanguage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<MenuDto>> GetMenus(string language, CancellationToken ct)
    {
        return await Mediator.Send(new PublicMenuList.Query(language), ct);
    }
}