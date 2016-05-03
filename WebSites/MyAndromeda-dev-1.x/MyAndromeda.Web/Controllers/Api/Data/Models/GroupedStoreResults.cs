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

    public class GroupedStoreResults
    {
        public int? StoreId { get; set; }
        public string ExternalSiteName { get; set; }
        public StoreSummaryModel[] DailyData { get; set; }

        public SummarySalesModelType Collection { get; set; }
        public SummarySalesModelType Delivery { get; set; }
        public SummarySalesModelType DineIn { get; set; }
        public SummarySalesModelType CarryOut { get; set; }
        public SummarySalesModelType Cancelled { get; set; }
        public SummarySalesModelType Total { get; set; }

        public List<StoreSummaryModel> WeekData { get; set; }
    }

}