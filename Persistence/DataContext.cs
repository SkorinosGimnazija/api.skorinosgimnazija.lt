namespace Persistence
{

using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; init; }
    }
}