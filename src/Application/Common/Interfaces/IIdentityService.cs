namespace SkorinosGimnazija.Application.Common.Interfaces;

using Authorization.Dtos;
using Domain.Entities.Identity;

public interface IIdentityService
{
    Task<UserAuthDto> AuthorizeAsync(string token);

    Task<AppUser?> GetOrCreateUserAsync(string? userName);
}