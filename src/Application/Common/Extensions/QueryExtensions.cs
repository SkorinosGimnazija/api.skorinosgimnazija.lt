namespace SkorinosGimnazija.Application.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Pagination;

public static class QueryExtensions
{
    public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> query, PaginationDto pagination)
    {
        var take = pagination.Items;
        var skip = take * pagination.Page;

        return query.Skip(skip).Take(take);
    }

    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IOrderedQueryable<T> query, PaginationDto pagination, CancellationToken ct = default)
    {
        var items = await query.Paginate(pagination).ToListAsync(ct);
        var count = await query.CountAsync(ct);

        return new(items, count, pagination.Page, pagination.Items);
    }
}