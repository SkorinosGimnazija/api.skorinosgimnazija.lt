namespace Persistence
{
    using Domain;
    using Domain.CMS;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; init; }

        public DbSet<Language> Languages { get; init; }

        public DbSet<Post> Posts { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            CreatePostModel(builder);
            CreateCategoryModel(builder);
            CreateLanguageModel(builder);
        }

        private static void CreateCategoryModel(ModelBuilder builder)
        {
            builder.Entity<Category>().HasIndex(x => x.Slug).IsUnique();
            builder.Entity<Category>().HasOne(x => x.Language).WithMany();
        }

        private static void CreateLanguageModel(ModelBuilder builder)
        {
            builder.Entity<Language>().HasIndex(x => x.Slug).IsUnique();
        }

        private static void CreatePostModel(ModelBuilder builder)
        {
            builder.Entity<Post>().HasOne(x => x.Category).WithMany();
        }
    }
}