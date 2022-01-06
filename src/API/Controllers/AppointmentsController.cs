namespace SkorinosGimnazija.API.Controllers;

using Base;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Appointments;
using SkorinosGimnazija.Infrastructure.Identity;
using System.Xml.Linq;
using SkorinosGimnazija.Application.Banners;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Application.Appointments;
using SkorinosGimnazija.Application.Menus;
using SkorinosGimnazija.Application.MenuLocations;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Application.ParentAppointments;
using SkorinosGimnazija.Application.ParentAppointments.Dtos;

[Authorize(Roles = Auth.Role.Teacher)]
public class AppointmentsController : BaseApiController
{
    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("all", Name = "GetAllAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDto>> GetAll([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentAdminList.Query(pagination), ct);
    }

    [HttpGet(Name = "GetMyAppointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<AppointmentDto>> GetAllMy([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentList.Query(pagination), ct);
    }

    //[HttpGet("{id:int}", Name = "GetAppointmentById")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<ActionResult<AppointmentDto>> Get(int id, CancellationToken ct)
    //{
    //    return await Mediator.Send(new AppointmentDetails.Query(id), ct);
    //}

    //[Authorize(Roles = Auth.Role.Admin)]
    //[HttpDelete("{id:int}", Name = "DeleteAppointment")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    await Mediator.Send(new AppointmentDelete.Command(id));
    //    return NoContent();
    //}

    [HttpGet("types", Name = "GetAppointmentTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<AppointmentTypeDto>> GetPublicMenuLocations(CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentTypesList.Query(), ct);
    }

    [AllowAnonymous]
    [HttpGet("public/time/{userName}", Name = "GetPublicAppointmentAvailableDates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<AppointmentDateDto>> GetAppointmentDates(string userName, CancellationToken ct)
    {
        return await Mediator.Send(new AppointmentDatesList.Query(new(userName, "parent")), ct);
    }
      
    //[AllowAnonymous]
    //[HttpPost("public", Name = "CreatePublicAppointment")]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult<AppointmentDto>> Create(AppointmentCreateDto dto)
    //{
    //    var result = await Mediator.Send(new AppointmentPublicCreate.Command(dto));
    //    return CreatedAtAction(nameof(Get), new { result.Id }, result);
    //}
}
