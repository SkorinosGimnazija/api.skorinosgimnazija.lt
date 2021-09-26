namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Dtos;
    using Application.Posts;
    using Application.Posts.Dtos;
    using Base;
    using Domain.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("posts")]
    [Authorize(Roles = AuthRole.Admin)]
    public class PostsController : BaseApiController
    {
        [HttpGet(Name = "GetPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<PostDto>> GetPosts([FromQuery] PaginationDto pagination, CancellationToken ct)
        {
            return await Mediator.Send(new PostList.Query(pagination), ct);
        }

        [HttpGet("{id:int}", Name = "GetPostById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostDetailsDto>> GetPost(int id, CancellationToken ct)
        {
            var entity = await Mediator.Send(new PostDetails.Query(id), ct);
            if (entity is null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPost(Name = "CreatePost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostDetailsDto>> CreatePost([FromForm] PostCreateDto post, CancellationToken ct)
        {
            var entity = await Mediator.Send(new PostCreate.Command(post), ct);

            return CreatedAtAction(nameof(GetPost), new { entity.Id }, entity);
        }

        [HttpPut(Name = "EditPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditPost([FromForm] PostEditDto post, CancellationToken ct)
        { 
            var result = await Mediator.Send(new PostEdit.Command(post), ct);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPatch("{id:int}", Name = "PatchPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchPost(int id, PostPatchDto post, CancellationToken ct)
        {
            var result = await Mediator.Send(new PostPatch.Command(id, post), ct);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
        {
            var result = await Mediator.Send(new PostDelete.Command(id), ct);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("search/{text}", Name = "SearchPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<PostDto>> SearchPosts(string text, [FromQuery] PaginationDto pagination,
            CancellationToken ct)
        {
            return await Mediator.Send(new PostSearchList.Query(text, pagination), ct);
        }

        [AllowAnonymous]
        [HttpGet("public/{id:int}", Name = "GetPublicPostById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [HttpGet("public/{language}", Name = "GetPublicPostsByLanguage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<PostDto>> GetPublicPosts(string language, [FromQuery] PaginationDto pagination,
            CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostList.Query(language, pagination), ct);
        }

        [AllowAnonymous]
        [HttpGet("public/{language}/search/{text:minlength(3)}", Name = "SearchPublicPostsByLanguageAndText")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<PostDto>> SearchPublicPosts(string language, string text,
            [FromQuery] PaginationDto pagination, CancellationToken ct)
        {
            return await Mediator.Send(new PublicPostSearchList.Query(language, text, pagination), ct);
        }
    }
}