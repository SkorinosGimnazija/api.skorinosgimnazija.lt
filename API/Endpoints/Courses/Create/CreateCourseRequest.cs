namespace API.Endpoints.Courses.Create;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using API.Database.Entities.Courses;
using JetBrains.Annotations;

[PublicAPI]
public record CreateCourseRequest
{
    [StringLength(CourseConfiguration.TitleLength)]
    public required string Title { get; init; }

    [StringLength(CourseConfiguration.OrganizerLength)]
    public required string Organizer { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }

    public required double DurationInHours { get; init; }

    [StringLength(CourseConfiguration.CertificateLength)]
    public string? Certificate { get; init; }

    public required bool IsUseful { get; init; }

    [FromClaim(JwtRegisteredClaimNames.Sub)]
    public required int CreatorId { get; init; }
}