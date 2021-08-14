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
using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = Roles.Admin)]
    public class PostsController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PublicPostDetailsDto>> GetPublicPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostDetails.Query(id), ct);
        }

        [AllowAnonymous]
        [HttpGet("{domain}/{language}")]
        public async Task<ActionResult<List<PublicPostDto>>> GetPublicPosts(string domain, string language, int pageNr,
            CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostList.Query(domain, language, pageNr), ct);
        }

        [AllowAnonymous]
        [HttpGet("{domain}/{language}/search/{text:minlength(3)}")]
        public async Task<ActionResult<List<PublicPostDto>>> SearchPublicPosts(string domain, string language, string text, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostSearchList.Query(domain, language, text), ct);
        }

        [HttpPost("admin")]
        public async Task<ActionResult<PostDetailsDto>> CreatePost(PostCreateDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostCreate.Command(post), ct);
        }

        [HttpPut("admin")]
        public async Task<IActionResult> EditPost( PostEditDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostEdit.Command( post), ct);
        }

        [HttpPatch("admin/{id:int}")]
        public async Task<IActionResult> PatchPost(int id, PostPatchDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostPatch.Command(id, post), ct);
        }

        [HttpDelete("admin/{id:int}")]
        public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostDelete.Command(id), ct);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<List<PostDto>>> GetPosts(int pageNr, CancellationToken ct)
        {
            return await Mediator.Send(new PostList.Query(pageNr), ct);
        }

        [HttpGet("admin/search/{text}")]
        public async Task<ActionResult<List<PostDto>>> SearchPosts(string text, CancellationToken ct)
        {
            return await Mediator.Send(new PostSearchList.Query(text), ct);
        }

        [HttpGet("admin/{id:int}")]
        public async Task<ActionResult<PostDetailsDto>> GetPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostDetails.Query(id), ct);
        }
    }
}