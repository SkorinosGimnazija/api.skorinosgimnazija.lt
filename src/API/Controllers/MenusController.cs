namespace SkorinosGimnazija.API.Controllers;

using Application.Menus;
using Application.Menus.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public sealed class MenusController : BaseApiController
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
        return await Mediator.Send(new MenuDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateMenu")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MenuDto>> CreateMenu(MenuCreateDto dto)
    {
        var result = await Mediator.Send(new MenuCreate.Command(dto));
        return CreatedAtAction(nameof(GetMenu), new { result.Id }, result);
    }

    [HttpPut(Name = "EditMenu")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditMenu(MenuEditDto dto)
    {
        await Mediator.Send(new MenuEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteMenu")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMenu(int id)
    {
        await Mediator.Send(new MenuDelete.Command(id));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("public/{language}/{location}", Name = "GetPublicMenusByLanguageAndLocation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<MenuDto>> GetMenus(string language, string location, CancellationToken ct)
    {
        return await Mediator.Send(new PublicMenuList.Query(language, location), ct);
    }
}