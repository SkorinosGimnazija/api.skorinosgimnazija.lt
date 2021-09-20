namespace Domain.Auth
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AuthRole
    {
        public const string Admin = "Admin";

        public const string Teacher = "Teacher";

        public const string BullyManager = "Bully";

        public const string Mod = "Mod";

        public static IEnumerable<string> GetAllRoles()
        {
            return typeof(AuthRole).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => (string) x.GetRawConstantValue()!);
        }
    }
}