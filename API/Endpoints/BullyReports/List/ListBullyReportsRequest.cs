namespace API.Endpoints.BullyReports.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListBullyReportsRequest : PaginationRequest
{
    [QueryParam]
    public int? UserId { get; init; }
}