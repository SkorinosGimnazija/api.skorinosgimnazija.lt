namespace Infrastructure.Auth;

using System.Security.Claims;
using Application.Interfaces;
using Domain.Auth;
using Microsoft.AspNetCore.Http;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal? _user;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _user = httpContextAccessor.HttpContext?.User;
    }

    public bool IsOwner(int ownerId)
    {
        if (!int.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return false;
        }

        return userId == ownerId;
    }

    public bool IsAdmin()
    {
        return HasRole(AuthRole.Admin);
    }

    public bool HasRole(string role)
    {
        return _user?.IsInRole(role) == true;
    }
}