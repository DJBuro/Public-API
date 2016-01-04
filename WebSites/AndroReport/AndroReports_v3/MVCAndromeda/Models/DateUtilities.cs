using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MVCAndromeda.Models
{
    public class DateUtilities
    {
        public static readonly string[] averagingPeriod = { "Day", "Week", "Month", "Year" };
        public static readonly string[] weekDayAveragingPeriod = { "Week Day for Week", "Week Day for Month", "Week Day for Year", "Month for Year" };
        public static readonly string[] weekDayShortAveragingPeriod = { "WkD/Wk", "WkD/M", "WkD/Y", "M/Y" };
        public static string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static string[] weekDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };


        //-------------------------get strings of year, months, day for the required term------------------------------------
        public static void ParseDate(DateTime today,string term, int unitsAgo, out string day, out string week, out string month, out string year,
            out int intWeek, out int intYear, out bool firstNonFullWeek, out bool lastNonFullWeek)
        {
            const int daysInWeek = 7;
            day = week = month = year = "";
            
            //today = Convert.ToDateTime("26 September 2013");//test
            DateTime date = today;

            if (!averagingPeriod.Contains(term) && !weekDayAveragingPeriod.Contains(term))
                throw new Exception(term + "Use only " + averagingPeriod.ToString() + weekDayAveragingPeriod.ToString() + " as term in CubeAdepter.getDailySummary");

            switch (term.Substring(term.LastIndexOf(' ') + 1))
            {
                case "Day":
                    date = today.AddDays(-unitsAgo);
                    break;
                case "Week":
                    date = today.AddDays(-unitsAgo * daysInWeek);
                    break;
                case "Month":
                    date = today.AddMonths(-unitsAgo);
                    break;
                case "Year":
                    date = today.AddYears(-unitsAgo);
                    break;
            }
            month = date.ToString("MMMM");
            year = "Calendar " + date.ToString("yyyy");
            intYear = date.Year;
            day = date.ToString("d ").Trim();
            intWeek = getWeekNumber(date);
            week = "Week " + intWeek.ToString();
            firstNonFullWeek = (intWeek == 1 && FirstDateOfWeek(intYear, intWeek).Day > 1) ? true : false;
            lastNonFullWeek = (intWeek == numOfLastWeek(intYear) && FirstDateOfWeek(intYear, intWeek).Day > 25) ? true : false;
        }
        public static int numOfLastWeek(int year)
        {
            return getWeekNumber(Convert.ToDateTime("31 December " + (year - 1).ToString()));
        }
        public static string getDateDescription(DateTime today, string term, int unitsAgo, Boolean fullFirstWeek = false)
        {
            string year, day, week, month;
            int intWeek, intYear;
            Boolean dummy;
            ParseDate(today, term, unitsAgo, out day, out week, out month, out year, out intWeek, out intYear, out dummy, out dummy);
            year = year.Substring(year.LastIndexOf(' ') + 1);
            week = week.Substring(week.LastIndexOf(' ') + 1);
            switch (term)
            {
                case "Day": return day + " " + month + ", " + year;
                case "Week": return FirstDateOfWeek(intYear, intWeek).ToString("d MMMM, yyyy");
                case "Month": return month + ", " + year;
                case "Year": return year;
            }
            return "";
        }
        public static int getWeekNumber(DateTime dateTime)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var weekNo = currentCulture.Calendar.GetWeekOfYear(
                dateTime,
                currentCulture.DateTimeFormat.CalendarWeekRule,
                currentCulture.DateTimeFormat.FirstDayOfWeek);
            return weekNo;
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = (jan1.DayOfWeek - DayOfWeek.Monday);
            if (daysOffset < 0) daysOffset += 7;

            DateTime firstMonday = jan1.AddDays(-daysOffset);
            return firstMonday.AddDays((weekOfYear - 1) * 7); ;
        }
    }
}