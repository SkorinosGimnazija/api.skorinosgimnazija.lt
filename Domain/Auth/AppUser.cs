namespace Domain.Auth;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser<int>
{
    public string? DisplayName { get; set; }
}