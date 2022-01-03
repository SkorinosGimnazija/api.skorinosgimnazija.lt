namespace SkorinosGimnazija.Application.Common.Interfaces;

using Domain.Entities.Identity;
using SkorinosGimnazija.Application.Authorization.Dtos;

public interface IIdentityService
{
    Task<UserAuthDto> AuthorizeAsync(string token);

    Task<AppUser?> GetOrCreateUserAsync(string? userName);
}