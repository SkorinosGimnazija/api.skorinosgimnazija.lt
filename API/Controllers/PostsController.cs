namespace API.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Posts;
    using Application.Posts.Dtos;
    using Domain.Auth;
    using Domain.CMS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = Roles.Admin)]
    public class PostsController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDto>> GetPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostDetails.Query(id), ct);
        }
         
        [AllowAnonymous]
        [HttpGet("{domain}/{language}")]
        public async Task<ActionResult<List<PostDto>>> GetPosts(string domain, string language, int pageNr,
            CancellationToken ct)
        {
            return await Mediator.Send(new PostList.Query(domain, language, pageNr), ct);
        }

        [HttpPost("admin")]
        public async Task<ActionResult<Post>> CreatePost(PostCreateDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostCreate.Command(post), ct);
        }

        [HttpPut("admin")]
        public async Task<IActionResult> EditPost( PostEditDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostEdit.Command( post), ct);
        }

        [HttpDelete("admin/{id:int}")]
        public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostDelete.Command(id), ct);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<List<Post>>> GetAdminPosts(int pageNr, CancellationToken ct)
        {
            return await Mediator.Send(new PostAdminList.Query(pageNr), ct);
        }

        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<Post>> GetAdminPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostAdminDetails.Query(id), ct);
        }
    }
}