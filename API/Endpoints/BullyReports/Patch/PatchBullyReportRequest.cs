namespace API.Endpoints.BullyReports.Patch;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.BullyReports;
using JetBrains.Annotations;

[PublicAPI]
public record PatchBullyReportRequest
{
    public required int Id { get; init; }

    [StringLength(BullyReportConfiguration.DetailsLength)]
    public string? Actions { get; init; }
}