namespace API.Controllers;

using Application.Categories;
using Application.Categories.Dtos;
using Base;
using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public sealed class CategoriesController : BaseApiController
{
    [HttpGet(Name = "GetCategories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<CategoryDto>> GetCategories(CancellationToken ct)
    {
        return await Mediator.Send(new CategoryList.Query(), ct);
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id, CancellationToken ct)
    {
        var result = await Mediator.Send(new CategoryDetails.Query(id), ct);
        if (result is null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpPost(Name = "CreateCategory")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryCreateDto category)
    {
        var result = await Mediator.Send(new CategoryCreate.Command(category));

        return CreatedAtAction(nameof(GetCategory), new { result.Id }, result);
    }

    [HttpPut(Name = "EditCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditCategory(CategoryEditDto category)
    {
        var result = await Mediator.Send(new CategoryEdit.Command(category));
        if (!result)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await Mediator.Send(new CategoryDelete.Command(id));
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}