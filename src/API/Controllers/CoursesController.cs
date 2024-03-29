﻿namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Pagination;
using Application.Courses;
using Application.Courses.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseStats = Application.Courses.CourseStats;

[Authorize(Roles = Auth.Role.Teacher)]
public class CoursesController : BaseApiController
{
    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("stats", Name = "GetCoursesStatsByDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<CourseStatsDto>> GetStats(
        [FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct)
    {
        return await Mediator.Send(new CourseStats.Query(start, end), ct);
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet("teacher/{id:int}", Name = "GetTeacherCoursesByIdAndDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<CourseDto>> GetByTeacher(
        int id, [FromQuery] DateTime start, [FromQuery] DateTime end, CancellationToken ct)
    {
        return await Mediator.Send(new CourseAdminList.Query(id, start, end), ct);
    }

    [HttpGet(Name = "GetMyCourses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<CourseDto>> GetMy([FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new CourseList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetCourseById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> Get(int id, CancellationToken ct)
    {
        return await Mediator.Send(new CourseDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateCourse")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CourseDto>> Create(CourseCreateDto dto)
    {
        var result = await Mediator.Send(new CourseCreate.Command(dto));
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut(Name = "EditCourse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(CourseEditDto dto)
    {
        await Mediator.Send(new CourseEdit.Command(dto));
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteCourse")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new CourseDelete.Command(id));
        return NoContent();
    }
}