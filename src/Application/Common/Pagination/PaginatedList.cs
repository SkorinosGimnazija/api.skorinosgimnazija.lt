namespace SkorinosGimnazija.Application.Common.Pagination;

public class PaginatedList<T>
{
    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        TotalCount = count;
    }

    public List<T> Items { get; }

    public int PageNumber { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    public bool HasPreviousPage
    {
        get { return PageNumber > 1; }
    }

    public bool HasNextPage
    {
        get { return PageNumber < TotalPages; }
    }
}