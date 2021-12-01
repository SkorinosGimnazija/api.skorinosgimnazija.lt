namespace SkorinosGimnazija.API.Controllers;

using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.Application.Banners.Dtos;
using SkorinosGimnazija.Application.Banners;
using SkorinosGimnazija.Infrastructure.Identity;
using System.Xml.Linq;
using Application.Banners;
using Application.Banners.Dtos;
 
[Authorize(Roles = Auth.Role.Admin)]
public class BannersController : BaseApiController
{
    [HttpGet(Name = "GetBanners")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<BannerDto>> GetAll(CancellationToken ct)
    {
        return await Mediator.Send(new BannerList.Query(), ct);
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
    public async Task<ActionResult<BannerDto>> Create(BannerCreateDto menu)
    {
        var result = await Mediator.Send(new BannerCreate.Command(menu));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut(Name = "EditBanner")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(BannerEditDto menu)
    {
        await Mediator.Send(new BannerEdit.Command(menu));
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
     
    [AllowAnonymous] 
    [HttpGet("public/{language}", Name = "GetPublicBannersByLanguage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<BannerDto>> GetAllPublic(string language, CancellationToken ct)
    {
        return await Mediator.Send(new PublicBannerList.Query(language), ct);
    }
}

