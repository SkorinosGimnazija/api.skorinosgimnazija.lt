namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Pagination;
using Application.Timetable;
using Application.Timetable.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.API.Controllers.Base;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Infrastructure.Identity;
using SkorinosGimnazija.Application.Accomplishments.Dtos;
using SkorinosGimnazija.Application.BullyJournal;
using System.Xml.Linq;
using Application.School.Dtos;

[Authorize(Roles = Auth.Role.Manager)]
public class TimetableController : BaseApiController
{
    [HttpGet(Name = "GetTimetable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<TimetableDto>> GetList(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new TimetableList.Query(pagination), ct);
    }
    
    [HttpGet("{id:int}", Name = "GetTimetableById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TimetableDto>> GetById(int id, CancellationToken ct)
    {
        return await Mediator.Send(new TimetableDetails.Query(id), ct);
    }


    [HttpPost(Name = "CreateTimetable")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TimetableDto>> Create(TimetableCreateDto dto)
    {
        var result = await Mediator.Send(new TimetableCreate.Command(dto));
        return CreatedAtAction(nameof(GetById), new { result.Id }, result);
    }

    [HttpPut(Name = "EditTimetable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(TimetableEditDto dto)
    {
        await Mediator.Send(new TimetableEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteTimetable")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new TimetableDelete.Command(id));
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("public/today", Name = "GetPublicTimetable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<TimetablePublicDto?> GetTodayList(CancellationToken ct)
    {
        return new()
        {
            Classtime = new()
            {
                StartTime = "9:35",
                EndTime = "10:20",
                Number = 2
            },
            Timetable = new()
            {
                new () {Id = 1, ClassRoom = "1 klasė", ClassName = "Lietuvių kalba"},
                new () {Id = 2, ClassRoom = "2 klasė", ClassName = "Matematika"},
                new () {Id = 3, ClassRoom = "3 klasė", ClassName = "Baltarusių kalba"},
                new () {Id = 4, ClassRoom = "4 klasė", ClassName = "Fizinis ugdymas"},
                new () {Id = 5, ClassRoom = "5a klasė", ClassName = "Baltarusių kalba"},
                new () {Id = 6, ClassRoom = "5b klasė", ClassName = "-"},
                new () {Id = 7, ClassRoom = "6a klasė", ClassName = "Etika"},
                new () {Id = 8, ClassRoom = "6b klasė", ClassName = "Geografija"},
                new () {Id = 9, ClassRoom = "7a klasė", ClassName = "Pasaulio pažinimas"},
                new () {Id = 10, ClassRoom = "7b klasė", ClassName = "Biologija"},
                new () {Id = 11, ClassRoom = "8a klasė", ClassName = "Istorija"},
                new () {Id = 12, ClassRoom = "8b klasė", ClassName = "Fizika"},
                new () {Id = 13, ClassRoom = "Ig klasė", ClassName = "Chemija"},
                new () {Id = 14, ClassRoom = "IIg klasė", ClassName = "Rusų kalba"},
                new () {Id = 15, ClassRoom = "IIIg klasė", ClassName = "Dailė ir technologijos"},
                new () {Id = 16, ClassRoom = "IVg klasė", ClassName = "Anglų kalba"},
            }
        };


        //return await Mediator.Send(new TimetablePublicList.Query(), ct);
    }
}
