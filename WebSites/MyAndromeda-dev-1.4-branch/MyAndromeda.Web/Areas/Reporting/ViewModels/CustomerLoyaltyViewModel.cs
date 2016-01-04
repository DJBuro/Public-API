using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class CustomerLoyaltyViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal? Points { get; set; }
        public decimal PendingPoints { get; set; }
        public decimal EarnedPoints { get; set; }
    }
}