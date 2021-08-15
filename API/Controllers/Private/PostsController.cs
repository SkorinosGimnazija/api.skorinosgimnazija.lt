using Application.Posts.Dtos;
using Application.Posts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Auth;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Private
{
    [Route("admin/posts")]
    [Authorize(Roles = Roles.Admin)]
    public class PostsController : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<PostDetailsDto>> CreatePost(PostCreateDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostCreate.Command(post), ct);
        }
    
        [HttpPut]
        public async Task<IActionResult> EditPost(PostEditDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostEdit.Command(post), ct);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchPost(int id,PostPatchDto post, CancellationToken ct)
        {
            return await Mediator.Send(new PostPatch.Command(id, post), ct);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostDelete.Command(id), ct);
        }

        [HttpGet("list/{pageNr:int:min(1)}")]
        public async Task<ActionResult<List<PostDto>>> GetPosts(int pageNr, CancellationToken ct)
        {
            return await Mediator.Send(new PostList.Query(pageNr), ct);
        }

        [HttpGet("search/{text}")]
        public async Task<ActionResult<List<PostDto>>> SearchPosts(string text, CancellationToken ct)
        {
            return await Mediator.Send(new PostSearchList.Query(text), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDetailsDto>> GetPost(int id, CancellationToken ct)
        {
            return await Mediator.Send(new PostDetails.Query(id), ct);
        }
    }
}
