using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using MyAndromeda.Data.DailyReporting.Services;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Threading.Tasks;
using MyAndromeda.Data.DataAccess.Chains;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Controllers.Api.Data
{
    public class DailyReportingDataController : ApiController
    {
        private readonly IChainDataService chainDataService;
        private readonly IStoreDataService storeDataService;
        private readonly IDailySummaryDataService dailySummaryDataService;
        private readonly IDateServices dateServices;

        public DailyReportingDataController(IDailySummaryDataService dailySummaryDataService,
            IStoreDataService storeDataService,
            IDateServices dateServices,
            IChainDataService chainDataService)
        {
            this.chainDataService = chainDataService;
            this.dateServices = dateServices;
            this.storeDataService = storeDataService;
            this.dailySummaryDataService = dailySummaryDataService;
        }

        [Route("chain-data/{chainId}/store/{andromedaSiteId}")]
        [HttpPost]
        public async Task<GroupedStoreResults> Data([FromUri]int chainId, [FromUri]int andromedaSiteId, DailyReportingQuery queryModel)
        {
            StoreParams store = await this.storeDataService.Table
                .Where(e => e.ChainId == chainId && e.AndromedaSiteId == andromedaSiteId)
                .Select(e => new StoreParams
                {
                    ExternalSiteName = e.ExternalSiteName,
                    AndromedaSiteId = e.AndromedaSiteId
                })
                .SingleOrDefaultAsync();

            StoreParams[] storeQuery = new[] { store };

            IEnumerable<GroupedStoreResults> data = await this.FetchData(storeQuery, queryModel);

            return data.First();
        }

        private async Task<IEnumerable<GroupedStoreResults>> FetchData(StoreParams[] storeData, DailyReportingQuery queryModel)
        {
            int[] storeIds = storeData.Select(e => e.AndromedaSiteId).ToArray();

            IQueryable<MyAndromeda.Data.DailyReporting.Model.CodeFirst.DailySummary> dataQuery = this.dailySummaryDataService.Table
                .Where(e => storeIds.Any(id => id == e.NStoreId));

            if (queryModel != null)
            {
                if (queryModel.From.HasValue)
                {
                    dataQuery = dataQuery.Where(e => e.TheDate > queryModel.From);
                }
                if (queryModel.To.HasValue)
                {
                    dataQuery = dataQuery.Where(e => e.TheDate < queryModel.To);
                }
            }

            //daily data from the database
            List<StoreSummaryModel> dataQueryResults = await dataQuery.Select(e => new StoreSummaryModel
            {
                CreateTimeStamp = e.TheDate,
                StoreId = e.NStoreId,
                Collection = new SummarySalesModelType()
                {
                    NetSales = e.OnlineColNetSales,
                    OrderCount = 0
                },
                Delivery = new SummarySalesModelType()
                {
                    NetSales = e.OnlineDelNetSales,
                    OrderCount = e.DelTotalOrders
                },
                DineIn = new SummarySalesModelType()
                {
                    NetSales = e.DineInNetSales,
                    OrderCount = e.DineInTotalOrders
                },
                CarryOut = new SummarySalesModelType()
                {
                    NetSales = e.CarryOutNetSales,
                    OrderCount = e.CarryOutTotalOrders
                },
                Cancelled = new SummarySalesModelType()
                {
                    NetSales = e.ValueCancels,
                    OrderCount = e.TotalCancels
                },
                Total = new SummarySalesModelType()
                {
                    NetSales = e.NetSales,
                    OrderCount = e.TotalOrders
                },
                AverageMakeTime = e.AvgMake,
                RackTime = e.RackTime,
                AverageOutTheDoorTime = e.AvgOutTheDoor,
                AverageToTheDoorTime = e.AvgDoorTime
            }).ToListAsync();

            dataQueryResults.ForEach((e) =>
            {
                e.ExternalSiteName = storeData
                    .Where(d => d.AndromedaSiteId == e.StoreId)
                    .Select(d => d.ExternalSiteName)
                    .FirstOrDefault();

                e.WeekOfYear = this.dateServices.GetWeekOfYear(e.CreateTimeStamp, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            });

            IEnumerable<IGrouping<int?, StoreSummaryModel>> groupedQuery = dataQueryResults.GroupBy(e => e.StoreId);

            //grouped and summed by the range for each store 
            List<GroupedStoreResults> groupedResult = groupedQuery.Select(e => new GroupedStoreResults()
            {
                StoreId = e.Key,
                ExternalSiteName = storeData
                    .Where(d => d.AndromedaSiteId == e.Key)
                    .Select(d => d.ExternalSiteName)
                    .FirstOrDefault(),

                DailyData = e.ToArray(),

                Collection = new SummarySalesModelType()
                {
                    NetSales = e.Sum(d => d.Collection.NetSales.GetValueOrDefault()),
                    OrderCount = e.Sum(d => d.Collection.OrderCount.GetValueOrDefault())
                },
                Delivery = new SummarySalesModelType()
                {
                    NetSales = e.Sum(d => d.Delivery.NetSales.GetValueOrDefault()),
                    OrderCount = e.Sum(d => d.Delivery.OrderCount.GetValueOrDefault()),
                },
                DineIn = new SummarySalesModelType()
                {
                    NetSales = e.Sum(d => d.DineIn.NetSales.GetValueOrDefault()),
                    OrderCount = e.Sum(d => d.OrderCount.GetValueOrDefault())
                },
                CarryOut = new SummarySalesModelType()
                {
                    NetSales = e.Sum(d => d.CarryOut.NetSales.GetValueOrDefault()),
                    OrderCount = e.Sum(d => d.CarryOut.OrderCount.GetValueOrDefault())
                },
                Cancelled = new SummarySalesModelType()
                {
                    NetSales = e.Sum(d => d.Cancelled.NetSales.GetValueOrDefault()),
                    OrderCount = e.Sum(d => d.Cancelled.OrderCount.GetValueOrDefault()),
                },
                Total = new SummarySalesModelType()
                {
                    NetSales = e.Sum(d => d.Total.NetSales.GetValueOrDefault()),
                    OrderCount = e.Sum(d => d.Total.OrderCount.GetValueOrDefault())
                },
                WeekData = e.GroupBy(d => d.WeekOfYear).OrderBy(d => d.Key).Select(week => new StoreSummaryModel()
                {
                    WeekOfYear = week.Key,
                    Collection = new SummarySalesModelType()
                    {
                        NetSales = week.Sum(d => d.Collection.NetSales.GetValueOrDefault()),
                        OrderCount = week.Sum(d => d.Collection.OrderCount.GetValueOrDefault())
                    },
                    Delivery = new SummarySalesModelType()
                    {
                        NetSales = week.Sum(d => d.Delivery.NetSales.GetValueOrDefault()),
                        OrderCount = week.Sum(d => d.Delivery.OrderCount.GetValueOrDefault()),
                    },
                    DineIn = new SummarySalesModelType()
                    {
                        NetSales = week.Sum(d => d.DineIn.NetSales.GetValueOrDefault()),
                        OrderCount = week.Sum(d => d.OrderCount.GetValueOrDefault())
                    },
                    CarryOut = new SummarySalesModelType()
                    {
                        NetSales = week.Sum(d => d.CarryOut.NetSales.GetValueOrDefault()),
                        OrderCount = week.Sum(d => d.CarryOut.OrderCount.GetValueOrDefault())
                    },
                    Cancelled = new SummarySalesModelType()
                    {
                        NetSales = week.Sum(d => d.Cancelled.NetSales.GetValueOrDefault()),
                        OrderCount = week.Sum(d => d.Cancelled.OrderCount.GetValueOrDefault()),
                    },
                    Total = new SummarySalesModelType()
                    {
                        NetSales = week.Sum(d => d.Total.NetSales.GetValueOrDefault()),
                        OrderCount = week.Sum(d => d.Total.OrderCount.GetValueOrDefault())
                    }
                    //Total = NetSales = new SummarySalesModelType()
                    //    {
                    //        NetSales = week.Sum(d => d.Total.NetSales.GetValueOrDefault()),
                    //        OrderCount = week.Sum(d => d.Total.OrderCount.GetValueOrDefault())
                    //    }
                    
                }).ToList()

            }).ToList();

            return groupedResult;
        }

        //[rout("Chain/{chainId}")]
        [Route("chain-data/{chainId}")]
        [HttpPost]
        public async Task<ChainResult> Data([FromUri]int chainId, DailyReportingQuery queryModel)
        {
            MyAndromeda.Data.Domain.Chain chain = this.chainDataService.Get(chainId);

            //var chain = this.chainDataService.Get(chainId);
            StoreParams[] storeData = await this.storeDataService.Table
                .Where(e => e.ChainId == chainId)
                .Select(e => new StoreParams { AndromedaSiteId = e.AndromedaSiteId, ExternalSiteName = e.ExternalSiteName })
                .ToArrayAsync();

            IEnumerable<GroupedStoreResults> groupedResult = (await this.FetchData(storeData, queryModel));

            var result = new ChainResult()
            {
                ChainId = chain.Id,
                ChainName = chain.Name,
                Data = groupedResult.ToList(),

                //WeekData = groupedResult.SelectMany(e => e.WeekData)
                //    .GroupBy(e => e.WeekOfYear),
                Cancelled = new SummarySalesModelType()
                {
                    NetSales = groupedResult.Sum(e => e.Cancelled.NetSales),
                    OrderCount = groupedResult.Sum(e => e.Cancelled.OrderCount)
                },
                CarryOut = new SummarySalesModelType()
                {
                    NetSales = groupedResult.Sum(e => e.CarryOut.NetSales),
                    OrderCount = groupedResult.Sum(e => e.CarryOut.OrderCount),
                },
                Collection = new SummarySalesModelType()
                {
                    NetSales = groupedResult.Sum(e => e.Collection.NetSales),
                    OrderCount = groupedResult.Sum(e => e.Collection.OrderCount)
                },
                Delivery = new SummarySalesModelType()
                {
                    NetSales = groupedResult.Sum(e => e.Delivery.NetSales),
                    OrderCount = groupedResult.Sum(e => e.Delivery.OrderCount)
                },
                DineIn = new SummarySalesModelType()
                {
                    NetSales = groupedResult.Sum(e => e.DineIn.NetSales),
                    OrderCount = groupedResult.Sum(e => e.DineIn.OrderCount)
                },
                Total = new SummarySalesModelType()
                {
                    NetSales = groupedResult.Sum(e => e.Total.NetSales),
                    OrderCount = groupedResult.Sum(e => e.Total.OrderCount)
                },
                WeekData = groupedResult.SelectMany(e => e.WeekData)
                    .GroupBy(e => e.WeekOfYear)
                    .Select(e => new StoreSummaryModel()
                    {
                        WeekOfYear = e.Key,
                        Total = new SummarySalesModelType()
                        {
                            NetSales = e.Sum(d => d.Total.NetSales),
                            OrderCount = e.Sum(d => d.Total.OrderCount)
                        },
                        OrderCount = e.Sum(d => d.OrderCount),
                        NetSales = e.Sum(d => d.NetSales),
                        DineIn = new SummarySalesModelType()
                        {
                            NetSales = e.Sum(d => d.DineIn.NetSales),
                            OrderCount = e.Sum(d => d.DineIn.OrderCount)
                        },
                        Delivery = new SummarySalesModelType()
                        {
                            NetSales = e.Sum(d => d.Delivery.NetSales),
                            OrderCount = e.Sum(d => d.Delivery.OrderCount),
                        },
                        Collection = new SummarySalesModelType()
                        {
                            NetSales = e.Sum(d => d.Collection.NetSales),
                            OrderCount = e.Sum(d => d.Collection.OrderCount)
                        },
                        CarryOut = new SummarySalesModelType()
                        {
                            NetSales = e.Sum(d => d.CarryOut.NetSales),
                            OrderCount = e.Sum(d => d.CarryOut.OrderCount)
                        },
                        Cancelled = new SummarySalesModelType() { 
                            NetSales = e.Sum(d=> d.Cancelled.NetSales),
                            OrderCount = e.Sum(d=> d.Cancelled.OrderCount)
                        }
                    })

                    .ToList()


            };

            return result;

            //return groupedResult;
        }

        


        
    }
}