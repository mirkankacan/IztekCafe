namespace IztekCafe.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDisplayString<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            return enumValue.ToString();
        }
    }
}