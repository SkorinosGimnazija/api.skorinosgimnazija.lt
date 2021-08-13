namespace Persistence
{
    using Domain.Auth;
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

        public DbSet<Menu> Menus { get; init; }

        public DbSet<Domain> Domains { get; init; }

        public DbSet<Post> Posts { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CreatePostModel(builder);
            CreateCategoryModel(builder);
            CreateLanguageModel(builder);
            CreateMenuModel(builder);
            CreateDomainModel(builder);
        }

        private static void CreateMenuModel(ModelBuilder builder)
        {
            builder.Entity<Menu>().HasOne(x => x.Category).WithMany();
            builder.Entity<Menu>().HasOne(x => x.Domain).WithMany();
            builder.Entity<Menu>().HasOne(x => x.ParentMenu).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Menu>().HasIndex(x => new { x.Slug, x.CategoryId, x.DomainId }).IsUnique();
        }

        private static void CreatePostModel(ModelBuilder builder)
        {
            builder.Entity<Post>().HasOne(x => x.Category).WithMany();
            builder.Entity<Post>().HasOne(x => x.Domain).WithMany();
        }

        private static void CreateCategoryModel(ModelBuilder builder)
        {
            builder.Entity<Category>().HasOne(x => x.Language).WithMany();
            builder.Entity<Category>().HasIndex(x => new { x.Slug, x.LanguageId }).IsUnique();
        }

        private static void CreateLanguageModel(ModelBuilder builder)
        {
            builder.Entity<Language>().HasIndex(x => x.Slug).IsUnique();
        }

        private static void CreateDomainModel(ModelBuilder builder)
        {
            builder.Entity<Domain>().HasIndex(x => x.Slug).IsUnique();
        }
    }
}