namespace Infrastructure.Auth;

using Application.Interfaces;
using Domain.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UserAccessor : IUserAccessor
{
    private readonly ClaimsPrincipal? _user;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
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
        return HasRole(Auth.Role.Admin);
    }

    public bool HasRole(string role)
    {
        return _user?.IsInRole(role) == true;
    }
}