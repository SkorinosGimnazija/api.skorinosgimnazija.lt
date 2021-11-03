﻿namespace Application.Extensions;

using Dtos;

internal static class QueryExtensions
{
    public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> query, PaginationDto pagination)
    {
        var take = pagination.Items;
        var skip = take * pagination.Page;

        return query.Skip(skip).Take(take);
    }
}