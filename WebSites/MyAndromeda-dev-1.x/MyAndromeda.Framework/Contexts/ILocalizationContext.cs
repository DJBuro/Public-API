using System;
using MyAndromeda.Core;

namespace MyAndromeda.Framework.Contexts
{
    public interface ILocalizationContext : IDependency
    {
        string CurrentCalendar { get; set; }

        string CurrentUiCulture { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }
    }
}