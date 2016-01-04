using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroCloudWebApiExternal.Contstraints
{
    public static class DateFormats
    {
        public static string ToStringAsUtc(this DateTime datetime) 
        {
            return datetime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}