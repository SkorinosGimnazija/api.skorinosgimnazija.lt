namespace API.Controllers;

using API.Controllers.Private;
using Application.Categories.Dtos;
using Application.Domains;
using Application.Features;
using Application.Interfaces;
using Application.Menus;
using Application.Posts;
using Application.Posts2;
using Domain.Auth;
using Domain.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
[Route("categories")]
[Authorize(Roles = Roles.Admin)]
public class CategoriesController : BaseApiController
{
    [HttpGet]
    public async Task<List<CategoryDto>> GetCategories(CancellationToken ct)
    {
        return await Mediator.Send(new CategoryList.Query(), ct);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id, CancellationToken ct)
    {
        var entity = await Mediator.Send(new CategoryDetails.Query(id), ct);
        if (entity is null)
        {
            return NotFound();
        }

        return entity;
    }
     
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryCreateDto category, CancellationToken ct)
    {
        var entity = await Mediator.Send(new CategoryCreate.Command(category), ct);
        
        return CreatedAtAction(nameof(GetCategory), new { entity.Id }, entity);
    }

    [HttpPut]
    public async Task<IActionResult> EditCategory(CategoryEditDto category, CancellationToken ct)
    {
        var result = await Mediator.Send(new CategoryEdit.Command(category), ct);
        if (!result)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
    {
        var result = await Mediator.Send(new CategoryDelete.Command(id), ct);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}