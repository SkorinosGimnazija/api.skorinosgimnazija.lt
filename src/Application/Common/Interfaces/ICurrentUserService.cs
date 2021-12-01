namespace SkorinosGimnazija.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }

    bool IsAdmin();

    bool HasRole(string role);

    bool IsResourceOwner(int resourceOwnerId);
}