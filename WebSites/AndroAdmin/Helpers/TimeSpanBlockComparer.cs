using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Helpers
{
    public class TimeSpanBlockComparer : Comparer<TimeSpanBlock>
    {
        public override int Compare(TimeSpanBlock x, TimeSpanBlock y)
        {
            return x.StartTime.CompareTo(y.StartTime);
        }
    }
}