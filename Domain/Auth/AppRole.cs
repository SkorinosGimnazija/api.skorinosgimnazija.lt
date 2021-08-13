namespace Domain.Auth
{
    using Microsoft.AspNetCore.Identity;

    public class AppRole : IdentityRole<int>
    {
        public AppRole(string roleName) : base(roleName)
        {
        }

        public AppRole()
        {
        }
    }
}