namespace SkorinosGimnazija.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser<int>
{
    public string DisplayName { get; set; } = default!;
}