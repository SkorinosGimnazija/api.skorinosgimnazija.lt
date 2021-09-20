namespace Application.Extensions
{
    using System.Linq;
    using Dtos;

    internal static class PaginationExtensions
    {
        public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> query, PaginationDto pagination)
        {
            var take = pagination.Items;
            var skip = take * pagination.Page;

            return query.Skip(skip).Take(take);
        }
    }
}