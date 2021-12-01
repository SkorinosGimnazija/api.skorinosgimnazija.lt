namespace SkorinosGimnazija.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> AuthorizeAsync(string token);
}