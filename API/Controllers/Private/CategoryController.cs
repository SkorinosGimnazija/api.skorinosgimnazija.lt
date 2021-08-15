using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
using Application.Domains.Dtos;
using Application.Domains;
using Application.Menus;
using Application.Posts;
using Application.Posts2;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading;
using Application.Categories.Dtos;
using Domain.CMS;

[Route("admin/categories")]
    [Authorize(Roles = Roles.Admin)]
    public class CategoryController : BaseApiController
    {
        [HttpGet("list")]
        public async Task<ActionResult<List<Category>>> GetCategories(CancellationToken ct)
        {
            return await Mediator.Send(new CategoryList.Query(), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GeCategory(int id, CancellationToken ct)
        {
            return await Mediator.Send(new CategoryDetails.Query(id), ct);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto category, CancellationToken ct)
        {
            return await Mediator.Send(new CategoryCreate.Command(category), ct);
        }

        [HttpPut]
        public async Task<IActionResult> EditCategory(CategoryEditDto category, CancellationToken ct)
        {
            return await Mediator.Send(new CategoryEdit.Command(category), ct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
        {
            return await Mediator.Send(new CategoryDelete.Command(id), ct);
        }
    }
}
