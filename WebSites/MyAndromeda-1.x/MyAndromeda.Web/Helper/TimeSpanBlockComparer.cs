﻿using MyAndromedaDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Helper
{
    public class TimeSpanBlockComparer : Comparer<TimeSpanBlock>
    {
        public override int Compare(TimeSpanBlock x, TimeSpanBlock y)
        {
            return x.StartTime.CompareTo(y.StartTime);
        }
    }
}