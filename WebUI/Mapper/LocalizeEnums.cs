using Microsoft.Extensions.Localization;

namespace WebUI.Mapper
{
    public static class EnumLocalizationExtensions
    {
        public static string Localize(this Enum value, IStringLocalizer localizer)
        {
            var key = value.ToString();
            return localizer[key];
        }
    }
}
