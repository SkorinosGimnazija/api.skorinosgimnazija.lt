namespace Domain
{

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Roles
{
    public const string Admin = "Admin";

    public const string BullyInfo = "Bully";

    public const string Mod = "Mod";

    public static IEnumerable<string> GetAllRoles()
    {
        return typeof(Roles).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(x => x.IsLiteral && !x.IsInitOnly)
            .Select(x => (string)x.GetRawConstantValue()!);
    }
}

}