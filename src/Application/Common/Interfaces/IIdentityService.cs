namespace SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Authorization.Dtos;

public interface IIdentityService
{
    Task<UserAuthDto> AuthorizeAsync(string token);
}