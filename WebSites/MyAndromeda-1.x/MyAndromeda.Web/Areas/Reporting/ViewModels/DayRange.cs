using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class DayRange
    {
        public DateTime? Start { get; set; }
        public int Days { get; set; }

        public string Name { get; set; }
    }
}