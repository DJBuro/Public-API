using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace MyAndromeda.Framework.Contexts
{
    public class LocalizationContext : ILocalizationContext 
    {
        readonly ICurrentChain currentChain;
        readonly ICurrentSite currentSite;

        public const string DefaultCulture = "en-GB";

        public static LocalizationContext Create(string culture, string timezoneId) 
        {
            return new LocalizationContext(culture, timezoneId);   
        }

        private LocalizationContext(string culture, string timezoneId) 
        {
            this.CurrentUiCulture = culture ?? DefaultCulture;
            this.CurrentTimeZone = string.IsNullOrWhiteSpace(timezoneId) ? TimeZoneInfo.Utc : TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
            this.CurrentCalendar = "GregorianCalendar";

            this.SetupThreadCulture();
        }

        public LocalizationContext(ICurrentChain currentChain, ICurrentSite currentSite) 
        {
            this.currentSite = currentSite;
            this.currentChain = currentChain;

            this.CurrentUiCulture = DefaultCulture;
            this.CurrentTimeZone = TimeZoneInfo.Utc;
            this.CurrentCalendar = "GregorianCalendar";
            
            this.Init();
            this.SetupThreadCulture();
        }
 
        private void SetupThreadCulture()
        {
            CultureInfo ci = new CultureInfo(this.CurrentUiCulture);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        private void Init() 
        {
            if (this.currentChain.Available) 
            {
                if (!string.IsNullOrWhiteSpace(currentChain.Chain.Culture)) 
                {
                    this.CurrentUiCulture = currentChain.Chain.Culture;
                }   
            }

            if (this.currentSite.Available) 
            {
                if (!string.IsNullOrWhiteSpace(currentSite.Store.UiCulture)) 
                { 
                    this.CurrentUiCulture = currentSite.Store.UiCulture;
                }
                if (!string.IsNullOrWhiteSpace(currentSite.Store.TimeZoneInfoId))
                {
                    this.CurrentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(currentSite.Store.TimeZoneInfoId);
                }
            }
        }

        public string CurrentCalendar
        {
            get;
            set;
        }

        public string CurrentUiCulture
        {
            get;
            set;
        }

        public TimeZoneInfo CurrentTimeZone
        {
            get;
            set;
        }
    }
}
