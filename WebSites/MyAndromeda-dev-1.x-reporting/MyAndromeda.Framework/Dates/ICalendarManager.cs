using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MyAndromeda.Framework.Dates
{
    public interface ICalendarManager : IDependency
    {
        /// <summary>
        /// Lists the calendars.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> ListCalendars();

        /// <summary>
        /// Gets the current calendar.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns></returns>
        Calendar GetCurrentCalendar(HttpContextBase requestContext);

        /// <summary>
        /// Gets the name of the calendar by.
        /// </summary>
        /// <param name="calendarName">Name of the calendar.</param>
        /// <returns></returns>
        Calendar GetCalendarByName(string calendarName);
    }
}
