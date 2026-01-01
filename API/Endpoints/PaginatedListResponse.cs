namespace API.Endpoints;

using JetBrains.Annotations;

[PublicAPI]
public sealed record PaginatedListResponse<T>
{
    public required List<T> Items { get; init; }

    public required int Page { get; init; }

    public required int TotalItems { get; init; }

    public required int TotalPages { get; init; }

    public bool HasPreviousPage
    {
        get { return Page > 1 && Page <= TotalPages; }
    }

    public bool HasNextPage
    {
        get { return Page < TotalPages; }
    }
}