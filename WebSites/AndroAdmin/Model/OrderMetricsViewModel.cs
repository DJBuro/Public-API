using AndroAdminDataAccess.Domain;
using DataWarehouseDataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AndroAdmin.Model
{
    public class OrderMetricsViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int? ChainId { get; set; }
        public int? ACSApplicationId { get; set; }
        public Guid? SiteId { get; set; }
        
        public IList<Chain> Chains { get; set; }
        public IList<Store> Stores { get; set; }
        public IList<ACSApplication> ACSApplications { get; set; }

        public OrderMetrics OrderMetrics { get; set; }
    }
}