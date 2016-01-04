using System;
using MyAndromeda.Core;

namespace MyAndromeda.Framework.Dates
{
    public interface IDateServices : IDependency
    {
        /// <summary>
        /// Converts a non-nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone.
        /// </summary>
        /// <param name="date">The non-nullable UTC date to convert. DateTime.MinValue is translated to null.</param>
        /// <returns></returns>
        DateTime? ConvertToLocalFromUtc(DateTime date);

        /// <summary>
        /// Converts a nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone.
        /// </summary>
        /// <param name="date">The nullable UTC date to convert.</param>
        /// <returns></returns>
        DateTime? ConvertToLocalFromUtc(DateTime? date);

        /// <summary>
        /// Converts a non-nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it using the default long date and time format string.
        /// </summary>
        /// <param name="date">The non-nullable UTC date to convert. DateTime.MinValue is translated to null.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date is equal to DateTime.MinValue.</param>
        /// <returns></returns>
        string ConvertToLocalString(DateTime date, string nullText = null);

        /// <summary>
        /// Converts a non-nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it using the specified format string using the MyAndromeda's configured culture.
        /// </summary>
        /// <param name="date">The non-nullable UTC date to convert. DateTime.MinValue is translated to null.</param>
        /// <param name="format">A standard DateTime format string to use for formatting the converted date.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date is equal to DateTime.MinValue.</param>
        /// <returns></returns>
        string ConvertToLocalString(DateTime date, string format, string nullText = null);

        /// <summary>
        /// Converts a nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it using the specified format string using the MyAndromeda's configured culture.
        /// </summary>
        /// <param name="date">The nullable UTC date to convert.</param>
        /// <param name="format">A standard DateTime format string to use for formatting the converted date.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date has no value.</param>
        /// <returns></returns>
        string ConvertToLocalString(DateTime? date, string format, string nullText = null);

        /// <summary>
        /// Converts a non-nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it as a date-only string using the MyAndromeda's configured culture.
        /// </summary>
        /// <param name="date">The non-nullable UTC date to convert. DateTime.MinValue is translated to null.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date is equal to DateTime.MinValue.</param>
        /// <returns></returns>
        string ConvertToLocalDateString(DateTime date, string nullText = null);

        /// <summary>
        /// Converts a nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it as a date-only string using the MyAndromeda's configured culture.
        /// </summary>
        /// <param name="date">The nullable UTC date to convert.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date has no value.</param>
        /// <returns></returns>
        string ConvertToLocalDateString(DateTime? date, string nullText = null);

        /// <summary>
        /// Converts a non-nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it as a time-only string using the MyAndromeda's configured culture.
        /// </summary>
        /// <param name="date">The non-nullable UTC date to convert. DateTime.MinValue is translated to null.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date is equal to DateTime.MinValue.</param>
        /// <returns></returns>
        string ConvertToLocalTimeString(DateTime date, string nullText = null);

        /// <summary>
        /// Converts a nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone and formats it as a time-only string using the MyAndromeda's configured culture.
        /// </summary>
        /// <param name="date">The nullable UTC date to convert.</param>
        /// <param name="nullText">A text to be returned if the supplied UTC date has no value.</param>
        /// <returns></returns>
        string ConvertToLocalTimeString(DateTime? date, string nullText = null);

        /// <summary>
        /// Converts a non-nullable date to Gregorian calendar UTC from the MyAndromeda's configured calendar and time zone.
        /// </summary>
        /// <param name="date">The non-nullable UTC date to convert. DateTime.MinValue is translated to null.</param>
        /// <returns></returns>
        DateTime? ConvertToUtcFromLocal(DateTime date);

        /// <summary>
        /// Converts a nullable date from Gregorian calendar UTC to the MyAndromeda's configured calendar and time zone.
        /// </summary>
        /// <param name="date">The nullable UTC date to convert.</param>
        /// <returns></returns>
        DateTime? ConvertToUtcFromLocal(DateTime? date);

        /// <summary>
        /// Parses a date and time string using the MyAndromeda's configured culture and converts it to Gregorian calendar UTC from the MyAndromeda's configured calendar and time zone.
        /// </summary>
        /// <param name="dateString">The local date and time string to parse and convert.</param>
        /// <returns></returns>
        DateTime? ConvertToUtcFromLocalString(string dateString);

        /// <summary>
        /// Parses separate date and time strings using the MyAndromeda's configured culture and converts the resulting combined DateTime to Gregorian calendar UTC from the MyAndromeda's configured calendar and time zone.
        /// </summary>
        /// <param name="dateString">The local date string to parse and convert, or null or an empty string to only convert the time component.</param>
        /// <param name="timeString">The local time string to parse and convert, or null or an empty string to only convert the date component.</param>
        /// <returns></returns>
        DateTime? ConvertToUtcFromLocalString(string dateString, string timeString);

        /// <summary>
        /// Converts from local string with custom format.
        /// </summary>
        /// <param name="dateValue">The date value.</param>
        /// <param name="customFormat">The custom format.</param>
        /// <returns></returns>
        DateTime? ConvertToUtcFromLocalStringWithCustomFormat(string dateValue, string customFormat);


    }
}