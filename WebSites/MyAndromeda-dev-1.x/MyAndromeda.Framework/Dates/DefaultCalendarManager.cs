using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Framework.Dates
{
    public class DefaultCalendarManager : ICalendarManager
    {
        readonly ILocalizationContext localizationContext;

        public DefaultCalendarManager(ILocalizationContext localizationContext)
        {
            this.localizationContext = localizationContext;
        }

        public IEnumerable<string> ListCalendars()
        {
            // Return all the calendar implementations in System.Globalization.
            // Could be done more dynamically using reflection, but that doesn't seem worth the performance hit.
            return new[] 
            {
                "ChineseLunisolarCalendar",
                "GregorianCalendar",
            };
        }

        public Calendar GetCurrentCalendar(HttpContextBase requestContext)
        {
            //get by request maybes 
            var requestCalendar = this.GetCalendarByName(localizationContext.CurrentCalendar);

            return requestCalendar;
        }

        public Calendar GetCalendarByName(string calendarName)
        {
            switch (calendarName)
            {
                case "ChineseLunisolarCalendar":
                    return new ChineseLunisolarCalendar();
                case "GregorianCalendar":
                    return new GregorianCalendar();
                default:
                    throw new ArgumentException(String.Format("The calendar name '{0}' is not a recognized System.Globalization calendar name.", calendarName), "calendarName");
            }
        }
    }
}