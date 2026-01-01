namespace API.Endpoints.BullyReports.Create;

using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using API.Database.Entities.BullyReports;
using JetBrains.Annotations;

[PublicAPI]
public record CreateBullyReportRequest
{
    public required DateOnly Date { get; init; }

    [StringLength(BullyReportConfiguration.NameLength)]
    public required string VictimName { get; init; }

    [StringLength(BullyReportConfiguration.NameLength)]
    public required string BullyName { get; init; }

    [StringLength(BullyReportConfiguration.LocationLength)]
    public required string Location { get; init; }

    [StringLength(BullyReportConfiguration.DetailsLength)]
    public required string Details { get; init; }

    [StringLength(BullyReportConfiguration.DetailsLength)]
    public string? Actions { get; init; }

    [FromClaim(JwtRegisteredClaimNames.Sub)]
    public required int CreatorId { get; init; }
}