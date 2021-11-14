namespace Domain.Auth;

using System.Reflection;

public static class Auth
{
    public const string NormalizedEmail = "@SKORINOSGIMNAZIJA.LT";

    public static IEnumerable<string> Roles
    {
        get
        {
            return typeof(Role).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral)
                .Select(x => (string)x.GetRawConstantValue()!);
        }
    }

    public static class Role
    {
        public const string Admin = "Admin";

        public const string Teacher = "Teacher";

        public const string BullyManager = "Bully";

        public const string Mod = "Mod";
    }
}