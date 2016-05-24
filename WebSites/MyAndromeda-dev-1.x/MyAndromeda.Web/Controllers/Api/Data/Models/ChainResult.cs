using System.Collections.Generic;

namespace MyAndromeda.Web.Controllers.Api.Data
{
    public class ChainResult
    {
        public int ChainId { get; set; }
        public string ChainName { get; set; }

        public List<GroupedStoreResults> Data { get; set; }


        public SummarySalesModelType Collection { get; set; }
        public SummarySalesModelType Delivery { get; set; }
        public SummarySalesModelType DineIn { get; set; }
        public SummarySalesModelType CarryOut { get; set; }
        public SummarySalesModelType Cancelled { get; set; }
        public SummarySalesModelType Total { get; set; }

        public List<StoreSummaryModel> WeekData { get; set; }
    }

}