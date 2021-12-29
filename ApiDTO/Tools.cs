using System.ComponentModel;

namespace DTO
{
    public static class Tools
    {
        public static string GetDescription<T>(this T member) where T : Enum
        {
            try
            {
                var enumType = member.GetType();
                var memberInfos = enumType.GetMember(member.ToString());
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = ((DescriptionAttribute)valueAttributes[0]).Description;
                return description;
            }
            catch (Exception)
            {
                return Enum.GetName(member.GetType(), member) ?? "";
            }
        }

    }
}
