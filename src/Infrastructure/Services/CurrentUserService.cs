namespace SkorinosGimnazija.Infrastructure.Services;

using System.Security.Claims;
using Application.Common.Interfaces;
using Identity;
using Microsoft.AspNetCore.Http;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User
    {
        get { return _httpContextAccessor.HttpContext?.User; }
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
        return IsResourceOwner(resourceOwnerId) || IsAdmin() || IsManager();
    }

    public bool IsResourceOwner(int resourceOwnerId)
    {
        return UserId == resourceOwnerId;
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