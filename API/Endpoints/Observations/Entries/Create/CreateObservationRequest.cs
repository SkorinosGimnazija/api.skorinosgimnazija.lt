namespace API.Endpoints.Observations.Entries.Create;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using API.Database.Entities.Observations;
using JetBrains.Annotations;

[PublicAPI]
public record CreateObservationRequest
{
    public required DateOnly Date { get; init; }

    [StringLength(ObservationConfiguration.NoteLength)]
    public string? Note { get; init; }

    public required int StudentId { get; init; }

    public required int LessonId { get; init; }

    public required List<int> OptionIds { get; init; }

    [FromClaim(JwtRegisteredClaimNames.Sub)]
    public required int CreatorId { get; init; }
}