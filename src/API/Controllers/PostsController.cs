namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Pagination;
using Application.Posts;
using Application.Posts.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Application.Menus;

[Authorize(Roles = Auth.Role.Admin)]
public sealed class PostsController : BaseApiController
{
    [HttpGet(Name = "GetPosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<PostDto>> GetPosts([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new PostList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetPostById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDetailsDto>> GetPost(int id, CancellationToken ct)
    {
        return await Mediator.Send(new PostDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreatePost")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PostDetailsDto>> CreatePost([FromForm] PostCreateDto dto)
    {
        var result = await Mediator.Send(new PostCreate.Command(dto));
        return CreatedAtAction(nameof(GetPost), new { result.Id }, result);
    }

    [HttpPut(Name = "EditPost")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditPost([FromForm] PostEditDto dto)
    {
        await Mediator.Send(new PostEdit.Command(dto));
        return Ok();
    }

    [HttpPatch("{id:int}", Name = "PatchPost")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchPost(int id, PostPatchDto dto)
    {
        await Mediator.Send(new PostPatch.Command(id, dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeletePost")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePost(int id)
    {
        await Mediator.Send(new PostDelete.Command(id));
        return NoContent();
    }

    [HttpGet("search/{text}", Name = "SearchPosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<PostDto>> SearchPosts(
        string text,
        [FromQuery] PaginationDto pagination,
        CancellationToken ct)
    {
        return await Mediator.Send(new PostSearch.Query(text, pagination), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/{id:int}", Name = "GetPublicPostById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDetailsDto>> GetPublicPost(int id, CancellationToken ct)
    {
        return await Mediator.Send(new PublicPostDetails.Query(id), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/path/{path}", Name = "GetPublicPostByMenuPath")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PostDetailsDto> GetPublicPost(string path, CancellationToken ct)
    {
        return await Mediator.Send(new PublicMenuLinkedPost.Query(path), ct);
    }
    
    [AllowAnonymous]
    [HttpGet("public/language/{language}", Name = "GetPublicPostsByLanguage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<PostDto>> GetPublicPosts(
        string language, [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new PublicPostList.Query(language, pagination), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/search/{text}", Name = "SearchPublicPosts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<PostDto>> SearchPublicPosts(
        string text, [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new PublicPostSearchList.Query(text, pagination), ct);
    }

 
}