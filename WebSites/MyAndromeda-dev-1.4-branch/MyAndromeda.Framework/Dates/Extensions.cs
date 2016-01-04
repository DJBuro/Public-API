using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Framework.Dates
{
    public static class Extensions
    {

        public static int GetWeekNumber(this System.DateTime? dateTime)
        {
            return dateTime.GetValueOrDefault().GetWeekNumber();
        }

        public static int GetWeekNumber(this System.DateTime dateTime)
        {

            var currentCulture = CultureInfo.CurrentCulture;
            var weekNum = currentCulture.Calendar
                .GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekNum;
        }
    }
}
