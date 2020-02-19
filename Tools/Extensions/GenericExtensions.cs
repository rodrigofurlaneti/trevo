using System.ComponentModel;

namespace Core.Extensions
{
    public static class GenericExtensions
    {
        public static string ToDescription<T>(this T source)
        {
            var fi = source.GetType().GetField(source.ToString());
            if (fi == null) return "";

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
    }
}
