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
using Application.Features;
using Application.Posts2;
using Application.Categories.Dtos;
using Domain.CMS;

namespace API.Controllers.Private
{
    [Route("posts")]
    [Authorize(Roles = Roles.Admin )]
    public class PostsController : BaseApiController
    {
        [HttpGet]
        public async Task<List<PostDto>> GetPosts([FromQuery]PaginationDto pagination, CancellationToken ct)
        {
            return await Mediator.Send(new PostList.Query(pagination), ct);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDetailsDto>> GetPost(int id, CancellationToken ct)
        {
            var entity = await Mediator.Send(new PostDetails.Query(id), ct);
            if (entity is null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<PostDetailsDto>> CreatePost([FromForm]PostCreateDto post, CancellationToken ct)
        { 
            var entity = await Mediator.Send(new PostCreate.Command(post), ct);

            return CreatedAtAction(nameof(GetPost), new { entity.Id }, entity);
        }
    
        [HttpPut]
        public async Task<IActionResult> EditPost(PostEditDto post, CancellationToken ct)
{
            var result = await Mediator.Send(new PostEdit.Command(post), ct);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchPost(int id,PostPatchDto post, CancellationToken ct)
        {
            var result = await Mediator.Send(new PostPatch.Command(id, post), ct);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new PostDelete.Command(id), ct);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
      
        [HttpGet("search/{text:minlength(3)}")]
        public async Task<List<PostDto>> SearchPosts(string text, [FromQuery]PaginationDto pagination, CancellationToken ct)
        {
            return await Mediator.Send(new PostSearchList.Query(text, pagination), ct);
        }

        [AllowAnonymous]
        [HttpGet("public/{id:int}")]
        public async Task<ActionResult<PostDetailsDto>> GetPublicPost(int id, CancellationToken ct)
        {
            var entity = await Mediator.Send(new PublicPostDetails.Query(id), ct);
            if (entity is null)
            {
                return NotFound();
            }

            return entity;
        }

        [AllowAnonymous]
        [HttpGet("public/{language}")]
        public async Task<List<PostDto>> GetPublicPosts(string language, [FromQuery] PaginationDto pagination, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostList.Query(language, pagination), ct);
        }

        [AllowAnonymous]
        [HttpGet("public/{language}/search/{text:minlength(3)}")]
        public async Task<List<PostDto>> SearchPublicPosts(string language, string text, [FromQuery] PaginationDto pagination, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostSearchList.Query(language, text, pagination), ct);
        }
    }
}
