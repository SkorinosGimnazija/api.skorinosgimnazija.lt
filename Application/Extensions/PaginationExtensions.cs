namespace Application.Extensions
{
    using Application.Features;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

  internal static  class PaginationExtensions
    {
        public static IQueryable<T> Paginate<T>(this IOrderedQueryable<T> query,PaginationDto pagination)
        {
            var take = pagination.Items;
            var skip = take * pagination.Page;
           
            return query.Skip(skip).Take(take);
        }
    }
}
