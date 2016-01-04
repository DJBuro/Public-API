using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.Model
{
    public class TimeZoneModel
    {
        public string DisplayText { get; set; }
        public string TimeZone { get; set; }

        public TimeZoneModel(string displayText, string timeZone)
        {
            this.DisplayText = displayText;
            this.TimeZone = timeZone;
        }
    }
}