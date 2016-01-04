using System;
using System.Globalization;
using System.Linq;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;

namespace MyAndromeda.Framework.Dates
{
    public class DateServicesFactory 
    {
        public DefaultDateService Create(LocalizationContext localizationContext) 
        {
            return DefaultDateService.Create(localizationContext);
        }
    }

    public class DefaultDateService : IDateServices
    {
        public static DefaultDateService Create(LocalizationContext localizationContext) 
        {
            var datetimeProvider = new CurrentDateTimeFormatProvider(localizationContext);
            var calenderManager = new DefaultCalendarManager(localizationContext);

            return new DefaultDateService(null, localizationContext, datetimeProvider, calenderManager);
        }

        private readonly IMyAndromedaLogger logger;

        private readonly ILocalizationContext localizationContext;
        private readonly IDateTimeFormatProvider dateTimeFormatProvider;
        private readonly ICalendarManager calendarManager;

        public DefaultDateService(
            IMyAndromedaLogger logger,
            ILocalizationContext localizationContext, IDateTimeFormatProvider dateTimeFormatProvider, ICalendarManager calendarManager) 
        {
            this.logger = logger;
            this.localizationContext = localizationContext;
            this.calendarManager = calendarManager;
            this.dateTimeFormatProvider = dateTimeFormatProvider;
        }

        public virtual DateTime? ConvertToLocalFromUtc(DateTime date)
        {
            return ConvertToLocalFromUtc(ToNullable(date));
        }

        public virtual DateTime? ConvertToLocalFromUtc(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            return TimeZoneInfo.ConvertTimeFromUtc(date.Value, localizationContext.CurrentTimeZone);
        }

        public virtual string ConvertToLocalString(DateTime date, string nullText = null)
        {
            return ConvertToLocalString(ToNullable(date), dateTimeFormatProvider.LongDateTimeFormat, nullText);
        }

        public virtual string ConvertToLocalString(DateTime date, string format, string nullText = null)
        {
            return ConvertToLocalString(ToNullable(date), format, nullText);
        }

        public virtual string ConvertToLocalString(DateTime? date, string format, string nullText = null)
        {
            var localDate = ConvertToLocalFromUtc(date);
            if (!localDate.HasValue)
            {
                return nullText;
            }

            // If the configured current calendar is different from the default calendar
            // of the configured current culture we must override the conversion process. 
            // We do this by using a custom CultureInfo modified to use GregorianCalendar
            // (means no calendar conversion will be performed as part of the string
            // formatting) and instead perform the calendar conversion ourselves.

            var cultureInfo = SiteCulture;
            var usingCultureCalendar = SiteCulture.DateTimeFormat.Calendar.GetType().IsInstanceOfType(SiteCalendar);
            if (!usingCultureCalendar)
            {
                cultureInfo = (CultureInfo)SiteCulture.Clone();
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
                var calendar = SiteCalendar;

                localDate = new DateTime(calendar.GetYear(localDate.Value), calendar.GetMonth(localDate.Value), calendar.GetDayOfMonth(localDate.Value), calendar.GetHour(localDate.Value), calendar.GetMinute(localDate.Value), calendar.GetSecond(localDate.Value));
            }

            return localDate.Value.ToString(format, cultureInfo);
        }

        public virtual string ConvertToLocalDateString(DateTime date, string nullText = null)
        {
            return ConvertToLocalDateString(ToNullable(date), nullText);
        }

        public virtual string ConvertToLocalDateString(DateTime? date, string nullText = null)
        {
            return ConvertToLocalString(date, dateTimeFormatProvider.ShortDateFormat, nullText);
        }

        public virtual string ConvertToLocalTimeString(DateTime date, string nullText = null)
        {
            return ConvertToLocalTimeString(ToNullable(date), nullText);
        }

        public virtual string ConvertToLocalTimeString(DateTime? date, string nullText = null)
        {
            return ConvertToLocalString(date, dateTimeFormatProvider.ShortTimeFormat, nullText);
        }

        public virtual DateTime? ConvertToUtcFromLocal(DateTime date)
        {
            return ConvertToUtcFromLocal(ToNullable(date));
        }

        public virtual DateTime? ConvertToUtcFromLocal(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            return TimeZoneInfo.ConvertTimeToUtc(date.Value, this.localizationContext.CurrentTimeZone);
        }

        public virtual DateTime? ConvertToUtcFromLocalString(string dateString)
        {
            if (String.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }

            // If the configured current calendar is different from the default calendar
            // of the configured current culture we must override the conversion process. 
            // We do this by using a custom CultureInfo modified to use GregorianCalendar
            // (means no calendar conversion will be performed as part of the string
            // parsing) and instead perform the calendar conversion ourselves.

            var cultureInfo = SiteCulture;
            var usingCultureCalendar = SiteCulture.DateTimeFormat.Calendar.GetType().IsInstanceOfType(SiteCalendar);
            if (!usingCultureCalendar)
            {
                cultureInfo = (CultureInfo)SiteCulture.Clone();
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
            }

            var localDate = DateTime.Parse(dateString, SiteCulture);

            if (!usingCultureCalendar)
            {
                var calendar = SiteCalendar;
                localDate = calendar.ToDateTime(localDate.Year, localDate.Month, localDate.Day, localDate.Hour, localDate.Minute, localDate.Second, localDate.Millisecond);
            }

            return ConvertToUtcFromLocal(localDate);
        }

        public virtual DateTime? ConvertToUtcFromLocalString(string dateString, string timeString)
        {
            

            if (String.IsNullOrWhiteSpace(dateString) && String.IsNullOrWhiteSpace(timeString))
            {
                return null;
            }

            // If the configured current calendar is different from the default calendar
            // of the configured current culture we must override the conversion process. 
            // We do this by using a custom CultureInfo modified to use GregorianCalendar
            // (means no calendar conversion will be performed as part of the string
            // parsing) and instead perform the calendar conversion ourselves.

            var cultureInfo = SiteCulture;
            var usingCultureCalendar = SiteCulture.DateTimeFormat.Calendar.GetType().IsInstanceOfType(SiteCalendar);
            if (!usingCultureCalendar)
            {
                cultureInfo = (CultureInfo)SiteCulture.Clone();
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
            }

            var localDate = !String.IsNullOrWhiteSpace(dateString) ? DateTime.Parse(dateString, cultureInfo) : new DateTime(1980, 1, 1);
            var localTime = !String.IsNullOrWhiteSpace(timeString) ? DateTime.Parse(timeString, cultureInfo) : new DateTime(1980, 1, 1, 12, 0, 0);
            var localDateTime = new DateTime(localDate.Year, localDate.Month, localDate.Day, localTime.Hour, localTime.Minute, localTime.Second);

            if (!usingCultureCalendar)
            {
                var calendar = SiteCalendar;
                localDateTime = calendar.ToDateTime(localDateTime.Year, localDateTime.Month, localDateTime.Day, localDateTime.Hour, localDateTime.Minute, localDateTime.Second, localDateTime.Millisecond);
            }

            return ConvertToUtcFromLocal(localDateTime);
        }

        public DateTime? ConvertToUtcFromLocalStringWithCustomFormat(string dateString, string customFormat)
        {
            Log(this.logger, "Convert to a datetime using format: {0}; value: {1}", customFormat, dateString);

            if(string.IsNullOrWhiteSpace(dateString)) 
            {
                return null;
            }

            var cultureInfo = this.SiteCulture;
            var usingCultureCalendar = SiteCulture.DateTimeFormat.Calendar.GetType().IsInstanceOfType(SiteCalendar);
            if (!usingCultureCalendar)
            {
                cultureInfo = (CultureInfo)this.SiteCulture.Clone();
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
            }

            DateTime localDate;
            if (DateTime.TryParseExact(dateString, customFormat, cultureInfo, DateTimeStyles.AssumeLocal, out localDate))
            {
                Log(this.logger, "Getting the local date worked!");
                var calendar = SiteCalendar;
                localDate = calendar.ToDateTime(localDate.Year, localDate.Month, localDate.Day, localDate.Hour, localDate.Minute, localDate.Second, localDate.Millisecond);

                var utcDateTime = this.ConvertToUtcFromLocal(localDate);

                if (!utcDateTime.HasValue)
                {
                    Log(this.logger, "failed to convert datetime using format: {0}; value: {1}", customFormat, dateString);
                }

                return utcDateTime;
            }
            else 
            {
                Log(this.logger, "failed to convert datetime using format: {0}; culture: {1}; value: {2}", customFormat, cultureInfo.DisplayName, dateString);
               
            }

            
            return null;
        }


        protected virtual CultureInfo SiteCulture
        {
            get
            {
                return CultureInfo.GetCultureInfo(this.localizationContext.CurrentUiCulture);
            }
        }

        protected virtual Calendar SiteCalendar
        {
            get
            {
                if (!String.IsNullOrEmpty(this.localizationContext.CurrentCalendar))
                {
                    return calendarManager.GetCalendarByName(this.localizationContext.CurrentCalendar);
                }
                return SiteCulture.Calendar;
            }
        }

        protected virtual DateTime? ToNullable(DateTime date)
        {
            return date == DateTime.MinValue ? new DateTime?() : new DateTime?(date);
        }

        private static void Log(IMyAndromedaLogger logger, string log, params string[] values) 
        {
            if (logger == null) { return; }
            logger.Debug(log, values);
        }
    }
}
