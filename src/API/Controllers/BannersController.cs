namespace SkorinosGimnazija.API.Controllers;

using Application.Banners;
using Application.Banners.Dtos;
using Application.Common.Pagination;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Admin)]
public class BannersController : BaseApiController
{
    [HttpGet(Name = "GetBanners")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<BannerDto>> GetAll([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new BannerList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetBannerById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BannerDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new BannerDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateBanner")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BannerDto>> Create([FromForm] BannerCreateDto dto)
    {
        var result = await Mediator.Send(new BannerCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut(Name = "EditBanner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit([FromForm] BannerEditDto dto)
    {
        await Mediator.Send(new BannerEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteBanner")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new BannerDelete.Command(id));
        return NoContent();
    }

    [HttpGet("search/{text}", Name = "SearchBanners")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<BannerDto>> Search(
        string text,
        [FromQuery] PaginationDto pagination,
        CancellationToken ct)
    {
        return await Mediator.Send(new BannerSearch.Query(text, pagination), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/{language}", Name = "GetPublicBannersByLanguage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<BannerDto>> GetAllPublic(string language, CancellationToken ct)
    {
        return await Mediator.Send(new PublicBannerList.Query(language), ct);
    }
}