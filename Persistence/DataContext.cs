namespace Persistence;

using Domain.Auth;
using Domain.CMS;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public sealed class DataContext : IdentityDbContext<AppUser, AppRole, int>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; init; } = default!;

    public DbSet<Language> Languages { get; init; } = default!;

    public DbSet<Menu> Menus { get; init; } = default!;
    public DbSet<MenuLocation> MenuLocations { get; init; } = default!;

    public DbSet<Post> Posts { get; init; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        CreatePostModel(builder);
        CreateCategoryModel(builder);
        CreateLanguageModel(builder);
        CreateMenuModel(builder);
        CreateMenuLocationModel(builder);
    }
   
    private static void CreateMenuModel(ModelBuilder builder)
    {
        builder.Entity<Menu>().HasOne(x => x.Language).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Menu>().HasOne(x => x.MenuLocation).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Menu>().HasOne(x => x.ParentMenu).WithMany().OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Menu>().HasIndex(x => x.Slug).IsUnique();
    }
    private static void CreateMenuLocationModel(ModelBuilder builder)
    {
        builder.Entity<MenuLocation>().HasIndex(x => x.Slug).IsUnique();
    }
    private static void CreatePostModel(ModelBuilder builder)
    {
        builder.Entity<Post>().HasOne(x => x.Category).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Post>().HasIndex(x => x.IsPublished);
        builder.Entity<Post>().HasIndex(x => x.PublishDate);
    }

    private static void CreateCategoryModel(ModelBuilder builder)
    {
        builder.Entity<Category>().HasOne(x => x.Language).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Category>().HasIndex(x => x.Slug).IsUnique();
    }

    private static void CreateLanguageModel(ModelBuilder builder)
    {
        builder.Entity<Language>().HasIndex(x => x.Slug).IsUnique();
    } 
}