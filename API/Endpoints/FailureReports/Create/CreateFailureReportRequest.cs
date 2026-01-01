namespace API.Endpoints.FailureReports.Create;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using API.Database.Entities.FailureReports;
using JetBrains.Annotations;

[PublicAPI]
public record CreateFailureReportRequest
{
    [StringLength(FailureReportConfiguration.LocationLength)]
    public required string Location { get; init; }

    [StringLength(FailureReportConfiguration.DetailsLength)]
    public required string Details { get; init; }

    [FromClaim(JwtRegisteredClaimNames.Sub)]
    public required int CreatorId { get; init; }
}