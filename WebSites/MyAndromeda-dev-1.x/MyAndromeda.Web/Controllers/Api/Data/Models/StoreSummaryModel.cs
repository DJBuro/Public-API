using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Data.Entity;
using System.Web.Http;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Threading.Tasks;
using MyAndromeda.Data.DataAccess.Chains;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Controllers.Api.Data
{

    public class StoreSummaryModel
    {
        public int? StoreId { get; set; }
        public string ExternalSiteName { get; set; }

        public SummarySalesModelType Collection { get; set; }
        public SummarySalesModelType Delivery { get; set; }
        public SummarySalesModelType DineIn { get; set; }
        public SummarySalesModelType CarryOut { get; set; }
        public SummarySalesModelType Cancelled { get; set; }
        public SummarySalesModelType Total { get; set; }

        public long? NetSales { get; set; }
        public long? OrderCount { get; set; }

        public long? AverageMakeTime { get; set; }
        public long? RackTime { get; set; }
        public long? AverageOutTheDoorTime { get; set; }

        public long? AverageToTheDoorTime { get; set; }

        public DateTime? CreateTimeStamp { get; set; }
        public int WeekOfYear { get; set; }
    }

}