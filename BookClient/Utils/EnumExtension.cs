using System.ComponentModel.DataAnnotations;

namespace Shared.Utils
{
    public static class EnumExtension
    {
        public static string GetDisplayName<T>(this T genre) where T : Enum
        {
            var displayAttribute = genre.GetType()
                .GetField(genre.ToString())
                ?.GetCustomAttributes(false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.Name ?? genre.ToString();
        }
    }
}
