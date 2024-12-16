namespace SkorinosGimnazija.Infrastructure.Identity;

using System.Reflection;
using System.Runtime.Serialization;

public static class Auth
{
    public static IEnumerable<string> AllRoles
    {
        get
        {
            return typeof(Role).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(x => x.IsLiteral && x.GetCustomAttribute<IgnoreDataMemberAttribute>() is null)
                .Select(x => (string) x.GetRawConstantValue()!);
        }
    }

    public static class Role
    {
        public const string Admin = "Admin";

        public const string Teacher = "Teacher";

        public const string SocialManager = "Social";

        public const string TechManager = "Tech";

        public const string Manager = "Manager";

        [IgnoreDataMember]
        public const string TeacherOrTechManager = Teacher + "," + TechManager;

        [IgnoreDataMember]
        public const string TeacherOrSocialManager = Teacher + "," + SocialManager;
    }
}