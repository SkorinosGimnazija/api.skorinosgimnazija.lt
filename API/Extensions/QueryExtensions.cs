namespace API.Extensions;

using API.Database.Entities.Authorization;
using API.Endpoints;

public static class QueryExtensions
{
    extension<T>(IQueryable<T> query)
    {
        public async Task<PaginatedListResponse<T>> ToPaginatedListAsync(
            PaginationRequest req,
            CancellationToken ct = default)
        {
            var items = await query.Skip(req.Skip).Take(req.Items).ToListAsync(ct);
            var count = await query.CountAsync(ct);

            return new()
            {
                Items = items,
                Page = req.Page,
                TotalItems = count,
                TotalPages = (count + req.Items - 1) / req.Items
            };
        }
    }

    extension(DbSet<AppUser> users)
    {
        public IOrderedQueryable<AppUser> Teachers()
        {
            return users
                .AsNoTracking()
                .Where(x => x.IsTeacher && !x.IsSuspended)
                .OrderBy(x => x.Name);
        }
    }
}