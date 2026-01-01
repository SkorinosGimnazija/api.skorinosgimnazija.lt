namespace API.Endpoints.BullyReports.Update;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.BullyReports;
using API.Endpoints.BullyReports.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateBullyReportRequest : CreateBullyReportRequest
{
    public required int Id { get; init; }

    [StringLength(BullyReportConfiguration.DetailsLength)]
    public string? Observers { get; init; }
}