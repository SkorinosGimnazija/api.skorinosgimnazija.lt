namespace SkorinosGimnazija.Application.Common.Interfaces;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

public interface IAppDbContext
{
    DbSet<Language> Languages { get; }

    DbSet<Menu> Menus { get; }

    DbSet<MenuLocation> MenuLocations { get; }

    DbSet<Post> Posts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}