using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Store.Models
{
    public class OpeningTimesViewModel
    {
        public string Day { get; set; }

        public bool OpenAllDay { get; set; }
        public bool ChangeIsOpen { get; set; }

        public DateTime? AddOpeningStartTime { get; set; }
        public DateTime? AddOpeningEndTime { get; set; }

        public OpeningTimesForTheWeek OpeningHours { get; set; }
    }
}