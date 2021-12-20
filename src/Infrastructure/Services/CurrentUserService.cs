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

    public bool IsOwnerOrAdmin(int resourceOwnerId)
    {
        return IsResourceOwner(resourceOwnerId) ||IsAdmin() ;
    }
     
    public bool IsResourceOwner(int resourceOwnerId)
    {
        return UserId == resourceOwnerId;
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