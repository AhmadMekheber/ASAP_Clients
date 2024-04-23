using System.ComponentModel.DataAnnotations;

namespace ASAP_Clients.Utilities
{
    public static class Extensions
    {
        public static string GetEnumDisplay(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = (DisplayAttribute?)fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)?.FirstOrDefault();

            return attribute?.Name ?? enumValue.ToString();
        }
    }
}