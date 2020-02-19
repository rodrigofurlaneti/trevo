using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class TimeSpanExtension
    {
        public static string ToStringTotalHour(this TimeSpan time)
        {
            return $"{Math.Floor(time.TotalHours).ToString().PadLeft(2, '0')}:{time.Minutes.ToString().PadLeft(2, '0')}";
        }
    }
}