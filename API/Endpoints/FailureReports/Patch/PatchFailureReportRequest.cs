namespace API.Endpoints.FailureReports.Patch;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using API.Database.Entities.FailureReports;
using JetBrains.Annotations;

[PublicAPI]
public record PatchFailureReportRequest
{
    public required int Id { get; init; }

    public bool? IsFixed { get; init; }

    [StringLength(FailureReportConfiguration.DetailsLength)]
    public string? Note { get; init; }

    [FromClaim(JwtRegisteredClaimNames.Sub)]
    public int FixerId { get; set; }
}