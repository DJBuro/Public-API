using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.Analytics.Models
{
    public class SummaryResult
    {
        public string Date { get; set; }
        public int Delivered { get; set; }
        public int Unsubscribes { get; set; }
        public int InvalidEmail { get; set; }
        public int Bounces { get; set; }
        public int RepeatUnsubscribes { get; set; }
        public int UniqueClicks { get; set; }
        public int Blocked { get; set; }
        public int SpamDrop { get; set; }
        public int RepeatBounces { get; set; }
        public int RepeatSpamReports { get; set; }
        public int Requests { get; set; }
        public int Spamreports { get; set; }
        public int Clicks { get; set; }
        public int Opens { get; set; }
        public int UniqueOpens { get; set; }
    }
}
