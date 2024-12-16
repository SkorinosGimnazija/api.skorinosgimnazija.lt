namespace SkorinosGimnazija.Infrastructure.Services;

using System.Security.Claims;
using Application.Common.Interfaces;
using Identity;
using Microsoft.AspNetCore.Http;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User
    {
        get { return httpContextAccessor.HttpContext?.User; }
    }

    public int UserId
    {
        get
        {
            if (int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
            {
                return id;
            }

            return -1;
        }
    }

    public string UserName
    {
        get { return User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty; }
    }

    public bool IsOwnerOrAdmin(int resourceOwnerId)
    {
        return IsResourceOwner(resourceOwnerId) || IsAdmin();
    }

    public bool IsOwnerOrManager(int resourceOwnerId)
    {
        return IsResourceOwner(resourceOwnerId) || IsManager();
    }

    public bool IsOwnerOrSocialManager(int resourceOwnerId)
    {
        return IsResourceOwner(resourceOwnerId) || IsSocialManager();
    }

    public bool IsOwnerOrTechManager(int resourceOwnerId)
    {
        return IsResourceOwner(resourceOwnerId) || IsTechManager();
    }

    public bool IsResourceOwner(int resourceOwnerId)
    {
        return UserId == resourceOwnerId;
    }

    public bool IsSocialManager()
    {
        return HasRole(Auth.Role.SocialManager);
    }

    public bool IsTechManager()
    {
        return HasRole(Auth.Role.TechManager);
    }

    public bool IsManager()
    {
        return HasRole(Auth.Role.Manager);
    }

    public bool IsAdmin()
    {
        return HasRole(Auth.Role.Admin);
    }

    public bool HasRole(string role)
    {
        return User?.IsInRole(role) == true;
    }
}