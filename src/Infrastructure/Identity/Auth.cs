namespace SkorinosGimnazija.Infrastructure.Identity;

using System.Reflection;

public static class Auth
{
    public static IEnumerable<string> AllRoles
    {
        get
        {
            return typeof(Role).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral)
                .Select(x => (string) x.GetRawConstantValue()!);
        }
    }

    public static class Role
    {
        public const string Admin = "Admin";

        public const string Teacher = "Teacher";

        public const string BullyManager = "Bully";

        public const string TechManager = "Tech";

        public const string Manager = "Manager";
    }
}