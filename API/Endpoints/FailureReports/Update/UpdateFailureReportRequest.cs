namespace API.Endpoints.FailureReports.Update;

using API.Endpoints.FailureReports.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateFailureReportRequest : CreateFailureReportRequest
{
    public required int Id { get; init; }
}