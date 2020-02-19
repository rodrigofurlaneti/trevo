using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class DateTimeExtension
    {
        public static int DiferencaDeMeses(this DateTime dataInicio, DateTime dataFim)
        {
            var meses = 0;

            while (dataInicio.AddMonths(1).Date <= dataFim.Date)
            {
                meses++;
                dataInicio = dataInicio.AddMonths(1);
            }

            return meses;
        }

        public static int GetMonthDifference(this DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        public static decimal TotalAnos(this DateTime dataInicio, DateTime dataFim)
        {
            var meses = dataInicio.DiferencaDeMeses(dataFim);

            var anos = Math.Floor(meses / 12d);
            var anosquebrados = (meses % 12d) / 100;
            var totalanos = anos + anosquebrados;
            return (decimal)totalanos;
        }
    }
}