namespace API.Endpoints.Observations.Entries.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListObservationsRequest : PaginationRequest
{
    [QueryParam]
    public int? CreatorId { get; init; }
}