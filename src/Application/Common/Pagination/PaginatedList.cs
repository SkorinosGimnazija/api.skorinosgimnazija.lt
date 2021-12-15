namespace SkorinosGimnazija.Application.Common.Pagination;

public class PaginatedList<T>
{
    public PaginatedList(IList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
    }
        
    public IList<T> Items { get; }

    public int PageNumber { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    public bool HasPreviousPage
    {
        get { return PageNumber > 1; }
    }

    public bool HasNextPage
    {
        get { return PageNumber < TotalPages - 1; }
    }
}