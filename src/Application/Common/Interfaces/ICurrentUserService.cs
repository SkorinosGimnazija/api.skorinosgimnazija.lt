namespace SkorinosGimnazija.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }

    string UserName { get; }

    bool IsOwnerOrAdmin(int resourceOwnerId);

    bool IsResourceOwner(int resourceOwnerId);

    bool IsAdmin();

    bool HasRole(string role);
}