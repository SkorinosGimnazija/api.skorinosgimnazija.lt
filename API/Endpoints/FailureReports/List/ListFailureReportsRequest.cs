namespace API.Endpoints.FailureReports.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListFailureReportsRequest : PaginationRequest
{
    [QueryParam]
    public required DateOnly StartDate { get; init; }

    [QueryParam]
    public required DateOnly EndDate { get; init; }
}