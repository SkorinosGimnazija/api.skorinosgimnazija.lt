namespace API.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Posts;
    using Domain;
    using Domain.CMS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Roles.Admin)]
    public class PostsController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post, CancellationToken ct)
        {
            return Ok(await Mediator.Send(new Create.Command(post), ct));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
        {
            return Ok(await Mediator.Send(new Delete.Command(id), ct));
        }

        [HttpPut]
        public async Task<IActionResult> EditPost(Post post, CancellationToken ct)
        {
            return Ok(await Mediator.Send(new Edit.Command(post), ct));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> GetPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new Details.Query(id), ct);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts(int page, CancellationToken ct)
        {
            return await Mediator.Send(new List.Query(page), ct);
        }
    }
}