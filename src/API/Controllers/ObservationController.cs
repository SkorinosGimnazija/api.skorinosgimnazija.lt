namespace SkorinosGimnazija.API.Controllers;

using Application.Common.Pagination;
using Application.Observation;
using Application.Observation.Dtos;
using Base;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = Auth.Role.Teacher)]
public class StudentObservationController : BaseApiController
{
    [HttpGet("types", Name = "GetObservationTypes")]
    [Tags("ObservationTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ObservationTypeDto>> GetObservationTypes(CancellationToken ct)
    {
        return await Mediator.Send(new ObservationTypeList.Query(), ct);
    }

    [HttpGet("types/{id:int}", Name = "GetObservationTypeById")]
    [Tags("ObservationTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationTypeDto>> GetObservationType(int id, CancellationToken ct)
    {
        return await Mediator.Send(new ObservationTypeDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("types", Name = "CreateObservationType")]
    [Tags("ObservationTypes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ObservationTypeDto>> CreateObservationType(
        ObservationTypeCreateDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(new ObservationTypeCreate.Command(dto), ct);
        return CreatedAtAction(nameof(GetObservationType), new { result.Id }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut("types", Name = "EditObservationType")]
    [Tags("ObservationTypes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditObservationType(ObservationTypeEditDto dto, CancellationToken ct)
    {
        await Mediator.Send(new ObservationTypeEdit.Command(dto), ct);
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("types/{id:int}", Name = "DeleteObservationType")]
    [Tags("ObservationTypes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteObservationType(int id, CancellationToken ct)
    {
        await Mediator.Send(new ObservationTypeDelete.Command(id), ct);
        return NoContent();
    }

    [HttpGet("lessons", Name = "GetObservationLessons")]
    [Tags("ObservationLessons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ObservationLessonDto>> GetObservationLessons(CancellationToken ct)
    {
        return await Mediator.Send(new ObservationLessonList.Query(), ct);
    }

    [HttpGet("lessons/{id:int}", Name = "GetObservationLessonById")]
    [Tags("ObservationLessons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationLessonDto>> GetObservationLesson(int id, CancellationToken ct)
    {
        return await Mediator.Send(new ObservationLessonDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("lessons", Name = "CreateObservationLesson")]
    [Tags("ObservationLessons")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ObservationLessonDto>> CreateObservationLesson(
        ObservationLessonCreateDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(new ObservationLessonCreate.Command(dto), ct);
        return CreatedAtAction(nameof(GetObservationLesson), new { result.Id }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut("lessons", Name = "EditObservationLesson")]
    [Tags("ObservationLessons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditObservationLesson(ObservationLessonEditDto dto, CancellationToken ct)
    {
        await Mediator.Send(new ObservationLessonEdit.Command(dto), ct);
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("lessons/{id:int}", Name = "DeleteObservationLesson")]
    [Tags("ObservationLessons")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteObservationLesson(int id, CancellationToken ct)
    {
        await Mediator.Send(new ObservationLessonDelete.Command(id), ct);
        return NoContent();
    }

    [HttpGet("targets", Name = "GetObservationTargets")]
    [Tags("ObservationTargets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ObservationTargetDto>> GetObservationTargets(
        [FromQuery] bool enabledOnly, CancellationToken ct)
    {
        return await Mediator.Send(new ObservationTargetList.Query(enabledOnly), ct);
    }

    [HttpGet("targets/{id:int}", Name = "GetObservationTargetById")]
    [Tags("ObservationTargets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ObservationTargetDto>> GetObservationTarget(int id, CancellationToken ct)
    {
        return await Mediator.Send(new ObservationTargetDetails.Query(id), ct);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPost("targets", Name = "CreateObservationTarget")]
    [Tags("ObservationTargets")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ObservationTargetDto>> CreateObservationTarget(
        ObservationTargetCreateDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(new ObservationTargetCreate.Command(dto), ct);
        return CreatedAtAction(nameof(GetObservationTarget), new { result.Id }, result);
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpPut("targets", Name = "EditObservationTarget")]
    [Tags("ObservationTargets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditObservationTarget(ObservationTargetEditDto dto, CancellationToken ct)
    {
        await Mediator.Send(new ObservationTargetEdit.Command(dto), ct);
        return Ok();
    }

    [Authorize(Roles = Auth.Role.Admin)]
    [HttpDelete("targets/{id:int}", Name = "DeleteObservationTarget")]
    [Tags("ObservationTargets")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteObservationTarget(int id, CancellationToken ct)
    {
        await Mediator.Send(new ObservationTargetDelete.Command(id), ct);
        return NoContent();
    }

    [Authorize(Roles = Auth.Role.Manager)]
    [HttpGet(Name = "GetStudentObservations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<StudentObservationDto>> GetStudentObservations(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new StudentObservationList.Query(pagination), ct);
    }

    [HttpGet("my", Name = "GetMyStudentObservations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PaginatedList<StudentObservationDto>> GetMyStudentObservations(
        [FromQuery] PaginationDto pagination, CancellationToken ct)
    {
        return await Mediator.Send(new StudentObservationMyList.Query(pagination), ct);
    }

    [HttpGet("{id:int}", Name = "GetStudentObservationById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentObservationDto>> GetStudentObservation(int id, CancellationToken ct)
    {
        return await Mediator.Send(new StudentObservationDetails.Query(id), ct);
    }

    [HttpPost(Name = "CreateStudentObservation")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StudentObservationDto>> CreateStudentObservation(
        StudentObservationCreateDto dto, CancellationToken ct)
    {
        var result = await Mediator.Send(new StudentObservationCreate.Command(dto), ct);
        return CreatedAtAction(nameof(GetStudentObservation), new { result.Id }, result);
    }

    [HttpPut(Name = "EditStudentObservation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EditStudentObservation(StudentObservationEditDto dto, CancellationToken ct)
    {
        await Mediator.Send(new StudentObservationEdit.Command(dto), ct);
        return Ok();
    }

    [HttpDelete("{id:int}", Name = "DeleteStudentObservation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudentObservation(int id, CancellationToken ct)
    {
        await Mediator.Send(new StudentObservationDelete.Command(id), ct);
        return NoContent();
    }
}