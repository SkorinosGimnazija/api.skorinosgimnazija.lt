namespace API.Services.Identity;

using System.Reflection;

public static class Auth
{
    public static class Role
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Teacher = "Teacher";
        public const string SocialManager = "Social";
        public const string TechManager = "Tech";

        public static IReadOnlyCollection<string> All { get; }
            = typeof(Role).GetFields(BindingFlags.Public |
                                     BindingFlags.Static |
                                     BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral)
                .Select(x => (string) x.GetRawConstantValue()!)
                .ToArray();
    }

    public static class Cookie
    {
        public const string RefreshTokenName = "refresh_token";

        public static CookieOptions RefreshTokenOptions(DateTime? expires)
        {
            return new()
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                Path = "/auth",
                SameSite = SameSiteMode.Lax,
                Expires = expires
            };
        }
    }
}