namespace API.Controllers;

using Application.Core.Dtos;
using Application.Posts;
using Application.Posts.Dtos;
using Base;
using Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public sealed class PostsController : BaseApiController
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
        var result = await Mediator.Send(new PostDetails.Query(id), ct);
        if (result is null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpPost(Name = "CreatePost")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PostDetailsDto>> CreatePost([FromForm] PostCreateDto post)
    {
        var result = await Mediator.Send(new PostCreate.Command(post));

        return CreatedAtAction(nameof(GetPost), new { result.Id }, result);
    }

    [HttpPut(Name = "EditPost")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditPost([FromForm] PostEditDto post)
    {
        var result = await Mediator.Send(new PostEdit.Command(post));
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
    public async Task<IActionResult> PatchPost(int id, PostPatchDto post)
    {
        var result = await Mediator.Send(new PostPatch.Command(id, post));
        if (!result)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeletePost")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await Mediator.Send(new PostDelete.Command(id));
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("search/{text}", Name = "SearchPosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<PostDto>> SearchPosts(string text, [FromQuery] PaginationDto pagination,
        CancellationToken ct)
    {
        return await Mediator.Send(new PostSearch.Query(text, pagination), ct);
    }

    [AllowAnonymous] 
    [HttpGet("public/{category}/{id:int}", Name = "GetPublicPostByCategoryAndId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDetailsDto>> GetPublicPost(string category, int id, CancellationToken ct)
    {
        var result = await Mediator.Send(new PublicPostDetails.Query(id, category), ct);
        if (result is null)
        {
            return NotFound();
        }

        return result;
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
    [HttpGet("public/search/{text}", Name = "SearchPublicPosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<PostDto>> SearchPublicPosts(string text, [FromQuery] PaginationDto pagination,
        CancellationToken ct)
    {
        return await Mediator.Send(new PublicPostSearchList.Query(text, pagination), ct);
    }
}