namespace SkorinosGimnazija.Application.Common.Interfaces;

public interface IRevalidationService
{
    Task<bool> RevalidateAsync(string language, string slug, int postId);

    Task<bool> RevalidateAsync(string language);
}