﻿using MyAndromedaDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Store.Models
{
    public enum Days { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    
    public class OpeningTimesForTheWeek : Dictionary<string, List<TimeSpanBlock>>
    {
        public IEnumerable<TimeSpanBlock> GetByDay(string day)
        {
            if (!this.ContainsKey(day)) { return Enumerable.Empty<TimeSpanBlock>(); }

            return this[day];
        }
        
        public IEnumerable<TimeSpanBlock> GetByDay(Days day) 
        {
            var key = day.ToString();

            return this.GetByDay(key);
        }

        public bool IsOpenOn(string day) 
        {
            if (!this.ContainsKey(day)) { return false; }

            return this[day.ToString()].Count > 0;
        }

        public bool IsOpenOn(Days day) 
        {
            return this.IsOpenOn(day.ToString());
        }
    }
}