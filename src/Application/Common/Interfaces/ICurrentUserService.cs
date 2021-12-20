namespace SkorinosGimnazija.Application.Common.Interfaces;

using System.Security.Claims;

public interface ICurrentUserService
{
    int UserId { get; }

    bool IsOwnerOrAdmin(int resourceOwnerId);

    bool IsResourceOwner(int resourceOwnerId);

    bool IsAdmin();

    bool HasRole(string role);
}